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
                NetworkManager.instance.Server.Clients[_fromClient].SpawnInGame(NetworkManager.setPName);

                //ENEMIES
                //load the enemies





                Debug.Log($"{NetworkManager.instance.Server.Clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully with client: {_fromClient} as {NetworkManager.setPName}.");
            }
            else
            {
                Debug.Log($"{NetworkManager.instance.Server.Clients[_fromClient].tcp.socket.Client.RemoteEndPoint} tried to login from client: {_fromClient} but failed to authorize.");
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
        NetworkManager.instance.Server.Clients[_fromClient].player.SetInput(_inputs, _rotation);
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
        NetworkManager.instance.Server.Clients[_fromCID].player.focus.target = null;
    }

    public static void RequestInteract(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        uint networkId = _packet.ReadUint();

        var component = NetworkManager.instance.FindNetworkComponent(networkId);
        if (component != null)
        {
            if(NetworkManager.instance.Server.Clients[_fromCID].player != null)
            {
                Vector3 distance = NetworkManager.instance.Server.Clients[_fromCID].player.transform.position - component.transform.position;
                component.GetComponent<IInteractable>().Interact(_fromCID, distance);
            }
            
        }
    }

    public static void OnUseItemRequested(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _itemID = _packet.ReadInt();
        string _itemName = _packet.ReadString();
        Inventory _playerInventory = NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Inventory>();

        for (int i = 0; i < _playerInventory.items.Count; i++)
        {
            if(_playerInventory.items[i].id == _itemID)
            {
                _playerInventory.items[i].Use(_fromCID);
                //We don't want to spam use items if we have multiple,
                //so just return out of the method after first use
                return;
            }
            else
            {
                Debug.Log("That item doesn't exist in the players inventory...");
            }
            
        }
    }

    public static void OnEquipItemRequested(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _itemID = _packet.ReadInt();
        Inventory _playerInventory = NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Inventory>();

        for (int i = 0; i < _playerInventory.items.Count; i++)
        {
            if (_playerInventory.items[i].id == _itemID)
            {
                Debug.Log($"{NetworkManager.instance.Server.Clients[_fromCID].player.username} equipped item {_playerInventory.items[i].name}");
                
                //We don't want to spam use items if we have multiple,
                //so just return out of the method after first use
                return;
            }
            else
            {
                Debug.Log("That item doesn't exist in the players inventory...");
            }

        }
    }

    public static void KillEnemy(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
    }
}

public enum InteractionType
{
    Item,
    Enemy,
    NPC,
    Teleport,
    Player
}