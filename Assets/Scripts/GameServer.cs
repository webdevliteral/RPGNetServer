using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class GameServer
{
    private int _port;
    private int _maxPlayers;
    //the tcplistener is functionally our server, which looks for incoming requests
    //and messages
    private TcpListener tcpListener;
    private UdpClient udpListener;
    private Dictionary<int, ClientRef> clients = new Dictionary<int, ClientRef>();
    public Dictionary<int, ClientRef> Clients => clients;

    private Dictionary<int, Action<int, Packet>> packetHandlers;
    public Dictionary<int, Action<int, Packet>> PacketHandlers => packetHandlers;
    public void StartServer(int maxPlayers, int port)
    {
        //set the server variables
        _port = port;
        _maxPlayers = maxPlayers;

        //Handle all initial server data
        InitializeServerData();

        //create our tcpListener
        tcpListener = new TcpListener(IPAddress.Any, this._port);
        //start and init the listener
        tcpListener.Start();
        //once listening, create a callback to handle messages
        //the null param can be passed an object
        //to send data when callback is complete
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        udpListener = new UdpClient(this._port);
        udpListener.BeginReceive(UDPReceiveCallback, null);
        Debug.Log($"Game Server successfully started!\n Running on port: {this._port}");
    }

    private void TcpConnectCallback(IAsyncResult _result)
    {
        //after received a request, create a copy of the client and stop listening
        //for that machine/client
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        //we want others to connect freely still, so restart the listener after
        //closing the connection
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        Debug.Log($" Incoming Game Server request from: {_client.Client.RemoteEndPoint}.");

        //LEFT OFF HERE
        for (int i = 1; i <= _maxPlayers; i++)
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

    private void UDPReceiveCallback(IAsyncResult _result)
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

    public void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
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

    private void InitializeServerData()
    {
        //PLAYER DATA
        for (int i = 1; i < _maxPlayers; i++)
        {
            clients.Add(i, new ClientRef(i));
        }
        
        packetHandlers = new Dictionary<int, Action<int, Packet>>()
        {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.userSessionConfirmed, ServerHandle.UserSessionReceived },
                { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                { (int)ClientPackets.requestFocus, ServerHandle.FocusGranted },
                { (int)ClientPackets.clearFocus, ServerHandle.ClearFocus },
                { (int)ClientPackets.killEnemy, ServerHandle.KillEnemy},
                { (int)ClientPackets.requestInteract, ServerHandle.RequestInteract},
                { (int)ClientPackets.requestUseItemSlot, ServerHandle.OnUseItemSlotRequested}
        };
        Debug.Log("Initialized packets.");
    }

    public void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}
