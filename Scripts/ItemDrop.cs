using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemDrop : Interactable
{
    public Item item;

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (base.Interact(_fromCID, _comparePosition))
        {
            Debug.Log($"Tryin to loot item: {item.name}");

            bool pickedUp = GameServer.clients[_fromCID].player.inventory.Add(item);

            if(pickedUp)
            {
                Destroy(ItemManager.instance.localItems[id]);
                ServerSend.ItemLooted(_fromCID, id, item.id);
                return true;
            }            
            
        }
            
        return false;
    }
}
