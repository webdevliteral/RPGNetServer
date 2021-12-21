using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemDrop : Interactable
{
    public Item item;

    private NetworkComponent _networkComponent;

    private void Start()
    {
        _networkComponent = GetComponent<NetworkComponent>();
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (base.Interact(_fromCID, _comparePosition))
        {
            Debug.Log($"Tryin to loot item: {item.name}");

            bool pickedUp = NetworkManager.instance.Server.Clients[_fromCID].player.inventory.Add(item);

            if(pickedUp)
            {
                if (ItemManager.instance.RemoveItemDrop(_networkComponent))
                {
                    Destroy(gameObject);
                    ServerSend.ItemLooted(_fromCID, _networkComponent.NetworkId, item.id);
                }

                return true;
            }            
        }
            
        return false;
    }
}
