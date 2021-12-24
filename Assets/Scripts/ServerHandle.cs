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
                component.GetComponent<IInteractable>().Interact(_fromCID, NetworkManager.instance.Server.Clients[_fromCID].player.transform.position);
            }
            
        }
    }

    public static void OnUseItemSlotRequested(int _fromClient, Packet _packet)
    {
        int fromCID = _packet.ReadInt();
        int slotIndex = _packet.ReadInt();
        Inventory playerInventory = NetworkManager.instance.Server.Clients[fromCID].player.GetComponent<Inventory>();

        if (slotIndex < 0 || slotIndex >= playerInventory.bagSpace)
        {
            Debug.Log($"Invalid slot index {slotIndex} received from {fromCID}");
        }

        playerInventory.items[slotIndex].Use(fromCID);
    }

    public static void KillEnemy(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
    }
}