using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class ItemDrop : Entity
{
    public Item item;

    private void Awake()
    {
        _networkComponent = GetComponent<NetworkComponent>();
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        Debug.Log($"Tryin to loot item: {item.name}");

        bool pickedUp = NetworkManager.instance.Server.Clients[_fromCID].player.inventory.Add(item);

        if (pickedUp)
        {
            if(NetworkManager.instance.RemoveNetworkComponent(_networkComponent))
            {
                Debug.Log("Removed item from world");
                Destroy(gameObject);
                ServerSend.ItemLooted(_fromCID, NetworkId, item.Id);
            }

            return true;
        }

        return false;
    }
}
