using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRef
{
    public static int dataBufferSize = 4096;

    
    public int CID;
    public Player player;
    public Enemy enemy;
    public TCP tcp;
    public UDP udp;

    //define client constructor
    public ClientRef(int _CID)
    {
        CID = _CID;
        tcp = new TCP(CID);
        udp = new UDP(CID);
    }

    //create a TCP "interface" for ease of use
    public class TCP
    {
        //store an instance of the client to send back for reference
        public TcpClient socket;
        private readonly int cid;
        private NetworkStream stream;
        private Packet receiveData;
        private byte[] receiveBuffer;
        public TCP(int _cid)
        {
            cid = _cid;
        }

        //connecting takes an instance of the client
        public void Connect(TcpClient _socket)
        {
            //set the client instance to whichever client we pass in
            socket = _socket;
            //make sure the socket is sending/receiving at proper rates
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            //read the whole stream of data
            stream = socket.GetStream();

            //interpret the packet
            receiveData = new Packet();

            //create a buffer for the data
            receiveBuffer = new byte[dataBufferSize];

            //finally, read the data
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            //TODO: send a response packet
            ServerSend.WelcomePacket(cid, "Authorizing login...");
            //ServerSend.LoginRequest(cid, "Login Server connected and ready!");
        }

        //Send some data using packets
        public void SendData(Packet _packet)
        {
            try
            {
                //make sure there is a connection before sending anything
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"ERROR SENDING DATA to player: {cid} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                //return an int representing the number of bytes read from stream
                int _byteLength = stream.EndRead(_result);
                //make sure some data actually made a transaction
                if (_byteLength <= 0)
                {
                    NetworkManager.instance.Server.Clients[cid].Disconnect();
                    //TODO: handle empty connections
                    return;
                }

                //if there is data though,
                //replicate our data into a new array with a length of byteLength
                byte[] _data = new byte[_byteLength];
                //then copy the data into our new array
                Array.Copy(receiveBuffer, _data, _byteLength);

                //TODO: handle that data
                HandleData(_data);
                receiveData.Reset();

                //FORNOW: just keep reading data from the stream
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log("ERROR: " + _ex);
                //TODO: properly disconnect the client on connection error
            }
        }

        private void HandleData(byte[] _data)
        {
            int _packetLength = 0;
            //set receivedData to the bytes from our tcp byteStream
            receiveData.SetBytes(_data);
            //check if there are more than 4 unread bytes. if yes, 
            //there is a new packet waiting because the first value
            //of a packet is also the length of the packet represented
            //by an int
            if (receiveData.UnreadLength() >= 4)
            {
                _packetLength = receiveData.ReadInt();
                if (_packetLength < 1)
                    //there isn't much left of the packet,
                    //so we return true in order to reset data
                    return;
            }

            while (_packetLength > 0 && _packetLength <= receiveData.UnreadLength())
            {
                //entering this loop means that we've got more bytes to read from another
                //packet. we'll copy the unread bytes to a new byte array to update the
                //data.
                byte[] _packetBytes = receiveData.ReadBytes(_packetLength);
                //on a new thread, create a new packet and assign it the proper id
                ThreadManager.ExecuteOnMainThread(() => {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        if (NetworkManager.instance.Server.PacketHandlers.TryGetValue((ClientPackets)_packetId, out var packetHandler))
                        {
                            packetHandler(cid, _packet);
                        }
                        else
                        {
                            Debug.LogWarning($"unknown packet id {_packetId}");
                        }
                    }
                });
            }
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receiveData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int cid;

        public UDP(int _cid)
        {
            cid = _cid;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
            //ServerSend.UserSession(cid);
        }

        public void SendData(Packet _packet)
        {
            NetworkManager.instance.Server.SendUDPData(endPoint, _packet);
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() => {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    NetworkManager.instance.Server.PacketHandlers[(ClientPackets)_packetId](cid, _packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SpawnInGame(string _playerName)
    {
        player = NetworkManager.instance.InstantiatePlayer();
        player.Initialize(CID, _playerName);
        foreach (ClientRef _client in NetworkManager.instance.Server.Clients.Values)
        {
            if (_client.player != null)
            {
                if (_client.CID != CID)
                {
                    ServerSend.SpawnPlayer(CID, _client.player);
                }
                else
                {
                    ServerSend.SpawnPlayer(_client.CID, player);
                }
            }
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected from the server.");
        
        ThreadManager.ExecuteOnMainThread(() => {
            UnityEngine.Object.Destroy(player.gameObject);
            player = null;
        });
        
        
        tcp.Disconnect();
        udp.Disconnect();

        ServerSend.PlayerDisconnected(CID);
    }
}
