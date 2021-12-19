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
        GameServer.clients[_toCID].tcp.SendData(_packet);
    }

    private static void SendUdpData(int _toCID, Packet _packet)
    {
        _packet.WriteLength();
        GameServer.clients[_toCID].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < GameServer.MaxPlayers; i++)
        {
            GameServer.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAllExceptOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < GameServer.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                GameServer.clients[i].tcp.SendData(_packet);
            }

        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < GameServer.MaxPlayers; i++)
        {
            GameServer.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAllExceptOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < GameServer.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                GameServer.clients[i].udp.SendData(_packet);
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
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toCID);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void UserSession(int _toCID, string _username)
    {
        using (Packet _packet = new Packet((int)ServerPackets.userSession))
        {
            _packet.Write(_toCID);
            _packet.Write(_username);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void SpawnPlayer(int _toCID, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTcpData(_toCID, _packet);
        }
    }

    //------------------------ NPC DATA

    public static void SpawnNPC(GameObject _npc)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnNPC))
        {
            _packet.Write(_npc.GetComponent<NPC>().id);
            _packet.Write(_npc.GetComponent<NPC>().entityName);
            _packet.Write(_npc.GetComponent<EntityStats>().maxHealth);
            _packet.Write(_npc.GetComponent<EntityStats>().currentHealth);
            _packet.Write(_npc.transform.position);
            _packet.Write(_npc.transform.rotation);

            SendTCPDataToAll(_packet);
        }
    }

    //ENEMY DATA
    public static void SpawnEnemy(GameObject _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            _packet.Write(_enemy.GetComponent<Enemy>().id);
            _packet.Write(_enemy.GetComponent<Enemy>().entityName);
            _packet.Write(_enemy.GetComponent<EnemyStats>().maxHealth);
            _packet.Write(_enemy.GetComponent<EnemyStats>().currentHealth);
            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SendEnemyData(int _toCID, GameObject _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.loadEnemiesOnClient))
        {
            _packet.Write(_enemy.GetComponent<Enemy>().id);
            _packet.Write(_enemy.GetComponent<Enemy>().entityName);
            _packet.Write(_enemy.GetComponent<EnemyStats>().maxHealth);
            _packet.Write(_enemy.GetComponent<EnemyStats>().currentHealth);
            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void SendNPCData(int _toCID, GameObject _npc)
    {
        using (Packet _packet = new Packet((int)ServerPackets.loadNPCsOnClient))
        {
            _packet.Write(_npc.GetComponent<NPC>().id);
            _packet.Write(_npc.GetComponent<NPC>().entityName);
            _packet.Write(_npc.GetComponent<EntityStats>().maxHealth);
            _packet.Write(_npc.GetComponent<EntityStats>().currentHealth);
            _packet.Write(_npc.transform.position);
            _packet.Write(_npc.transform.rotation);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void UpdateEnemyStats(GameObject _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateEnemyStats))
        {
            _packet.Write(_enemy.GetComponent<Enemy>().id);
            _packet.Write(_enemy.GetComponent<EntityStats>().currentHealth);

            SendTCPDataToAll(_packet);
        }
    }

    public static void UpdateEnemyPosition(GameObject _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateEnemyPosition))
        {
            _packet.Write(_enemy.GetComponent<Enemy>().id);
            _packet.Write(_enemy.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void KillEnemy(GameObject _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.killEnemy))
        {
            Debug.Log("KILL ENEMY: " + _enemy.GetComponent<Enemy>().id);
            _packet.Write(_enemy.GetComponent<Enemy>().id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(Player _player)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAllExceptOne(_player.id, _packet);
        }
    }

    public static void PlayerDisconnected(int _pid)
    {
        //create an instance of a packet
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_pid);

            SendTCPDataToAll(_packet);
        }
    }

    public static void InteractableTooFar(int _toCID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.interactableTooFar))
        {
            _packet.Write("You need to move closer to interact with that!");

            SendTcpData(_toCID, _packet);
        }
    }

    public static void InteractionConfirmed(int _toCID, int _eID, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.interactionConfirmed))
        {
            _packet.Write(_toCID);
            _packet.Write(_eID);
            _packet.Write(_msg);

            SendTcpData(_toCID, _packet);
        }
    }

    public static void SendClientsLootData(int _itemID, int _eID, Vector3 _lootSpawnPos)
    {
        using (Packet _packet = new Packet((int)ServerPackets.sendClientsLootData))
        {
            _packet.Write(_itemID);
            _packet.Write(_eID);
            _packet.Write(_lootSpawnPos);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ItemLooted(int _fromCID, int _eID, int _itemID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemLooted))
        {
            _packet.Write(_fromCID);
            _packet.Write(_eID);
            _packet.Write(_itemID);

            SendTCPDataToAll(_packet);
        }
    }
    public static void ItemEquipped(int _toCID, int _itemID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemEquipped))
        {
            _packet.Write(_toCID);
            _packet.Write(_itemID);

            SendTcpData(_toCID, _packet);
        }
    }
}
