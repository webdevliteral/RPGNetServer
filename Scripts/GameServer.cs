using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class GameServer
{
    static string _consoleTitle = "RPGNet Logic/Game Server";
    public static int Port { get; private set; }
    public static int MaxPlayers { get; private set; }
    //the tcplistener is functionally our server, which looks for incoming requests
    //and messages
    private static TcpListener tcpListener;
    private static UdpClient udpListener;
    public static Dictionary<int, ClientRef> clients = new Dictionary<int, ClientRef>();

    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    public static void StartServer(int _maxPlayers, int _port)
    {
        //set the server variables
        Port = _port;
        MaxPlayers = _maxPlayers;
        Console.Title = _consoleTitle;

        //Handle all initial server data
        InitializeServerData();

        //create our tcpListener
        tcpListener = new TcpListener(IPAddress.Any, Port);
        //start and init the listener
        tcpListener.Start();
        //once listening, create a callback to handle messages
        //the null param can be passed an object
        //to send data when callback is complete
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);
        Debug.Log($"Game Server successfully started!\n Running on port: {Port}");
    }

    private static void TcpConnectCallback(IAsyncResult _result)
    {
        //after received a request, create a copy of the client and stop listening
        //for that machine/client
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        //we want others to connect freely still, so restart the listener after
        //closing the connection
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        Debug.Log($" Incoming Game Server request from: {_client.Client.RemoteEndPoint}.");

        //LEFT OFF HERE
        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }

        }

        //HandleServerFull();
        //if we can't add anymore connections,
        //handle that
        //TODO: actually handle SERVER_MAX_SIZE
        Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect! Server is full!");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();
                if (_clientId == 0)
                    return;
                if (clients[_clientId].udp.endPoint == null)
                {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    //this provides client server authority to make sure the sender isn't impersonating anyone
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error receiving UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending UDP data to {_clientEndPoint}: {_ex}");
        }
    }

    private static void InitializeServerData()
    {
        //PLAYER DATA
        for (int i = 1; i < MaxPlayers; i++)
        {
            clients.Add(i, new ClientRef(i));
        }

        //ENEMY DATA
        EnemyManager.instance.InitEnemyData();

        //NPC DATA
        NPCManager.instance.InitNPCData();
        
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.userSessionConfirmed, ServerHandle.UserSessionReceived },
                { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                { (int)ClientPackets.requestFocus, ServerHandle.FocusGranted },
                { (int)ClientPackets.clearFocus, ServerHandle.ClearFocus },
                { (int)ClientPackets.killEnemy, ServerHandle.KillEnemy},
                { (int)ClientPackets.requestInteract, ServerHandle.RequestInteract},
                { (int)ClientPackets.requestLoot, ServerHandle.OnLootRequested},
                { (int)ClientPackets.requestUseItem, ServerHandle.OnUseItemRequested},
                { (int)ClientPackets.requestEquipItem, ServerHandle.OnEquipItemRequested}
        };
        Debug.Log("Initialized packets.");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}
