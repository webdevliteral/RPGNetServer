using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    //once the packet is created, we need to send the data through a tcp socket
    //this will prepare the packet for sending and ultimately finalize the signal
    private static void SendTcpData(int _toCID, Packet _packet)
    {
        _packet.WriteLength();
        NetworkManager.instance.Server.Clients[_toCID].tcp.SendData(_packet);
    }

    private static void SendUdpData(int _toCID, Packet _packet)
    {
        _packet.WriteLength();
        NetworkManager.instance.Server.Clients[_toCID].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < NetworkManager.instance.Server.Clients.Count; i++)
        {
            if (NetworkManager.instance.Server.Clients[i].player != null)
                NetworkManager.instance.Server.Clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAllExceptOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < NetworkManager.instance.Server.Clients.Count; i++)
        {
            if (i != _exceptClient)
            {
                if (NetworkManager.instance.Server.Clients[i].player != null)
                    NetworkManager.instance.Server.Clients[i].tcp.SendData(_packet);
            }

        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < NetworkManager.instance.Server.Clients.Count; i++)
        {
            if (NetworkManager.instance.Server.Clients[i].player != null)
                NetworkManager.instance.Server.Clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAllExceptOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < NetworkManager.instance.Server.Clients.Count; i++)
        {
            if (i != _exceptClient)
            {
                if (NetworkManager.instance.Server.Clients[i].player != null)
                    NetworkManager.instance.Server.Clients[i].udp.SendData(_packet);
            }

        }
    }

    //this is where we'll create methods to create packets which will be sent over the server
    //for example chat packets, move packets, interact packets, etc

    //NOTE: send order is VERY important. please take note of how you're sending and
    //receiving data.

    //CID = Client ID. 
    //See "ClientRef.cs"
    public static void WelcomePacket(int _toCID, string _msg)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet(ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toCID);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void UserSession(int _toCID, string _username)
    {
        using (Packet _packet = new Packet(ServerPackets.userSession))
        {
            _packet.Write(_toCID);
            _packet.Write(_username);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void SpawnPlayer(int _toCID, Player _player)
    {
        using (Packet _packet = new Packet(ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);
            _packet.Write(_player.NetworkId);

            SendTcpData(_toCID, _packet);
        }
    }

    //------------------------ ENTITY DATA

    public static void SpawnEntity(uint networkId, uint prefabId, Vector3 position, Quaternion rotation)
    {
        using (Packet _packet = new Packet(ServerPackets.spawnEntity))
        {
            _packet.Write(networkId);
            _packet.Write(prefabId);
            _packet.Write(position);
            _packet.Write(rotation);

            SendTCPDataToAll(_packet);
        }
            
    }

    public static void UpdateEntityStats(uint networkId, int healthValue)
    {
        using (Packet _packet = new Packet(ServerPackets.updateEntityStats))
        {
            _packet.Write(networkId);
            _packet.Write(healthValue);

            SendTCPDataToAll(_packet);
        }
    }

    public static void UpdateEnemyPosition(uint networkId, Vector3 _position)
    {
        using (Packet _packet = new Packet(ServerPackets.updateEntityPosition))
        {
            _packet.Write(networkId);
            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet(ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(Player _player)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet(ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAllExceptOne(_player.id, _packet);
        }
    }

    public static void PlayerDisconnected(int _pid)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet(ServerPackets.playerDisconnected))
        {
            _packet.Write(_pid);

            SendTCPDataToAll(_packet);
        }
    }

    public static void InteractableTooFar(int _toCID)
    {
        using (Packet _packet = new Packet(ServerPackets.interactableTooFar))
        {
            _packet.Write("You need to move closer to interact with that!");

            SendTcpData(_toCID, _packet);
        }
    }

    public static void InteractionConfirmed(int _toCID, string _msg)
    {
        using (Packet _packet = new Packet(ServerPackets.interactionConfirmed))
        {
            _packet.Write(_toCID);
            _packet.Write(_msg);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void SendClientsLootData(int _prefabId, int _itemID, uint _networkId, Vector3 _lootSpawnPos)
    {
        using (Packet _packet = new Packet(ServerPackets.sendClientsLootData))
        {
            _packet.Write(_prefabId);
            _packet.Write(_itemID);
            _packet.Write(_networkId);
            _packet.Write(_lootSpawnPos);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ItemLooted(int _fromCID, uint _networkId, int _itemID)
    {
        using (Packet _packet = new Packet(ServerPackets.itemLooted))
        {
            _packet.Write(_fromCID);
            _packet.Write(_networkId);
            _packet.Write(_itemID);

            SendTCPDataToAll(_packet);
        }
    }
    public static void ItemEquipped(int _toCID, int _itemID)
    {
        using (Packet _packet = new Packet(ServerPackets.itemEquipped))
        {
            _packet.Write(_toCID);
            _packet.Write(_itemID);

            SendTcpData(_toCID, _packet);
        }
    }
}
