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

            bool pickedUp = NetworkManager.instance.Server.Clients[_fromCID].player.inventory.Add(item);

            if(pickedUp)
            {
                ServerSend.ItemLooted(_fromCID, id, item.id);
                for (int i = 0; i < ItemManager.instance.localItems.Count; i++)
                {
                    if (ItemManager.instance.localItems[i].GetComponent<ItemDrop>().id == id)
                    {
                        Destroy(ItemManager.instance.localItems[i]);
                        ItemManager.instance.localItems.Remove(ItemManager.instance.localItems[i]);
                        //We don't want to spam use items if we have multiple,
                        //so just return out of the method after first use
                        return true;
                    }
                    else
                    {
                        Debug.Log("That item doesn't exist in the players inventory...");
                    }
                }
                    
                return true;
            }            
            
        }
            
        return false;
    }
}
