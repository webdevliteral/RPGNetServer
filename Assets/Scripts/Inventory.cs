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

    public event Action<Item> OnItemChanged;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    public bool Add(Item _item)
    {
        if(items.Count >= bagSpace)
        {
            Debug.Log("Not enough inventory room.");
            return false;
        }
        items.Add(_item);

        OnItemChanged?.Invoke(_item);

        Debug.Log($"ID: {_item} was added to {_player.username}s inventory.");
        return true;
    }

    public void Remove(Item item)
    {
        
        items.Remove(item);
        OnItemChanged?.Invoke(item);
    }
}
