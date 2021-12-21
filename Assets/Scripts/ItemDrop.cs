using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemDrop : NetworkComponent
{
    public Item item;

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (base.Interact(_fromCID, _comparePosition))
        {
            Debug.Log($"Tryin to loot item: {item.name}");

            bool pickedUp = NetworkManager.instance.Server.Clients[_fromCID].player.inventory.Add(item);

            if(pickedUp)
            {
                if (ItemManager.instance.RemoveItemDrop(this))
                {
                    Destroy(gameObject);
                    ServerSend.ItemLooted(_fromCID, NetworkId, item.id);
                }

                return true;
            }            
        }
            
        return false;
    }
}
