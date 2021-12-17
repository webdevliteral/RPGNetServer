using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        //read everything from the packet
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _password = _packet.ReadString();


        try
        {
            if (NetworkManager.AttemptNewSession(_username, _password))
            {
                //PLAYER DATA
                ServerSend.UserSession(_fromClient, NetworkManager.setPName);
                GameServer.clients[_fromClient].SpawnInGame(NetworkManager.setPName);

                //ENEMIES
                //load the enemies
                EnemyManager.instance.LoadEnemiesOnClient(_fromClient);




                Debug.Log($"{GameServer.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully with client: {_fromClient} as {NetworkManager.setPName}.");
            }
            else
            {
                Debug.Log($"{GameServer.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} tried to login from client: {_fromClient} but failed to authorize.");
            }
        }
        catch (Exception _ex)
        {
            Debug.Log(_ex);
        }

        //check if the clientid matches
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID (Expected: {_clientIdCheck})");
        }

        //The welcome transaction was finished and received, so send the player into the game
    }

    public static void UserSessionReceived(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Debug.Log(_msg);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();
        GameServer.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    public static void FocusGranted(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
        string _eName = _packet.ReadString();

        //set focus
        Debug.Log($"Client {_fromCID} is trying to focus entity: {_eID} with EntityName: {_eName}");
        

    }

    public static void ClearFocus(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        Debug.Log($"Client {_fromCID} cleared their focus.");
        GameServer.clients[_fromCID].player.focus.target = null;
    }

    public static void RequestAttack(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
        int _damage = _packet.ReadInt();
        Debug.Log($"Client {_fromCID} is trying to attack enemy: {EnemyManager.enemies[_eID].gameObject.name} for {_damage}.");

        EnemyManager.enemies[_eID].GetComponent<EnemyStats>().TakeDamage(_eID, _damage);
    }

    public static void RequestInteract(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
        string _type = _packet.ReadString();
        Vector3 _comparePosition = GameServer.clients[_fromCID].player.transform.position;

        

        if (_type == "Enemy")
        {
            Debug.Log($"Client {_fromCID} is trying to interact with an {_type}: {EnemyManager.enemies[_eID].gameObject.name} from position {_comparePosition}.");
            EnemyManager.enemies[_eID].GetComponent<Enemy>().Interact(_fromCID, _comparePosition);
        }
        else if(_type == "Item")
        {
            Debug.Log($"Client {_fromCID} is trying to interact with an {_type}: {ItemManager.instance.localItems[_eID]} from position {_comparePosition}.");
            ItemManager.instance.localItems[_eID].GetComponent<ItemDrop>().Interact(_fromCID, _comparePosition);
        }
    }

    public static void OnLootRequested(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _itemID = _packet.ReadInt();
        Vector3 _comparePosition = GameServer.clients[_fromCID].player.transform.position;

        Debug.Log($"Client {_fromCID} is trying to loot item with ID: {_itemID}.");
        //TODO: create a list of global items somewhere so i can
        //add a global reference to the players instance of inventory
        ItemManager.instance.localItems[_itemID].GetComponent<ItemDrop>().Interact(_fromCID, _comparePosition);
        //GameServer.clients[_fromCID].player.inventory.Add(GlobalItemDB.instance.globalItems[_itemID]);
    }

    public static void OnUseItemRequested(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _itemID = _packet.ReadInt();
        string _itemName = _packet.ReadString();

        for(int i = 0; i < GameServer.clients[_fromCID].player.GetComponent<Inventory>().items.Count; i++)
        {
            if(GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[i].id == _itemID)
            {
                Debug.Log($"{GameServer.clients[_fromCID].player.username} used item {_itemName}");
                GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[i].Use(_fromCID);
                //We don't want to spam use items if we have multiple,
                //so just return out of the method after first use
                return;
            }
            else
            {
                Debug.Log("That item doesn't exist in the players inventory...");
            }
            
        }
        //GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[].id;
    }

    public static void OnEquipItemRequested(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _itemID = _packet.ReadInt();

        for (int i = 0; i < GameServer.clients[_fromCID].player.GetComponent<Inventory>().items.Count; i++)
        {
            if (GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[i].id == _itemID)
            {
                Debug.Log($"{GameServer.clients[_fromCID].player.username} equipped item {GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[i].name}");
                
                //We don't want to spam use items if we have multiple,
                //so just return out of the method after first use
                return;
            }
            else
            {
                Debug.Log("That item doesn't exist in the players inventory...");
            }

        }
        //GameServer.clients[_fromCID].player.GetComponent<Inventory>().items[].id;
    }

    public static void KillEnemy(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
    }
}
