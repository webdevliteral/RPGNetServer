using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private NetworkList<ItemDrop> itemDrops = new NetworkList<ItemDrop>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void AddItemDrop(ItemDrop itemDrop)
    {
        itemDrops.Add(itemDrop);
    }

    public bool RemoveItemDrop(ItemDrop itemDrop)
    {
        return itemDrops.Remove(itemDrop);
    }

    public ItemDrop FindItemDrop(uint networkId)
    {
        var itemDrop = itemDrops.GetByNetworkId(networkId);
        if (itemDrop != null)
        {
            return itemDrop.GetComponent<ItemDrop>();
        }

        return null;
    }
}
