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
                ServerSend.UserSession(_fromClient, NetworkManager.instance.playerName);
                NetworkManager.instance.Server.Clients[_fromClient].InitializeInGame(NetworkManager.instance.playerName);

                Debug.Log($"{NetworkManager.instance.Server.Clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully with client: {_fromClient} as {_username}.");
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
        uint networkId = _packet.ReadUint();

        NetworkManager.instance.Server.Clients[_fromCID].player.focus.target = NetworkManager.instance.FindNetworkComponent(networkId).gameObject.transform;
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

    public static void OnUseSpellRequested(int _fromClient, Packet _packet)
    {
        int fromCID = _packet.ReadInt();
        int activeSpellId = _packet.ReadInt();
        NetworkManager.instance.Server.Clients[fromCID].player.GetComponent<ActiveSpellbook>().Spells[activeSpellId].Use(fromCID);
    }

    public static void OnQuestAccepted(int _fromClient, Packet _packet)
    {
        int fromCID = _packet.ReadInt();
        int questIdToAccept = _packet.ReadInt();
        if (QuestAtlas.instance.AllQuests[questIdToAccept] != null)
        {
            if (NetworkManager.instance.Server.Clients[fromCID].player.CurrentQuests.FindQuestInQuestlog(questIdToAccept))
                return;

            Quest original = QuestAtlas.instance.AllQuests[questIdToAccept];
            Quest questReference = ScriptableObject.CreateInstance<Quest>();
            questReference.Initialize(original.id, original.title, original.description, original.rewards, original.experience, original.currencyReward, original.Objectives);

            NetworkManager.instance.Server.Clients[fromCID].player.CurrentQuests.AddQuest(questReference);
        }
            
    }

    public static void KillEnemy(int _fromClient, Packet _packet)
    {
        int _fromCID = _packet.ReadInt();
        int _eID = _packet.ReadInt();
    }
}