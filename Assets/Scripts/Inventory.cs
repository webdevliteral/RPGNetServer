using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int bagSpace = 20;
    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public bool Add(Item _item)
    {
        if(items.Count >= bagSpace)
        {
            Debug.Log("Not enough inventory room.");
            return false;
        }
        items.Add(_item);
        
        onItemChangedCallback?.Invoke();

        Debug.Log($"ID: {_item} was added to {GetComponent<Player>().username}s inventory.");
        return true;
    }

    public void Remove(Item item)
    {
        
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }
}
