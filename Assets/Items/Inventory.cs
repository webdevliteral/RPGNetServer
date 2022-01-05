using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int bagSpace = 20;
    public List<Item> items = new List<Item>();

    public event Action<Item> OnQuestItemLooted;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public bool Add(Item _item)
    {
        if(items.Count >= bagSpace)
        {
            Debug.Log("Not enough inventory room.");
            return false;
        }
        items.Add(_item);

        if(_item.Type == ItemType.QuestItem)
            OnQuestItemLooted?.Invoke(_item);

        Debug.Log($"ID: {_item} was added to {player.username}s inventory.");
        return true;
    }

    public void Remove(Item item)
    {
        
        items.Remove(item);
        OnQuestItemLooted?.Invoke(item);
    }
}
