using UnityEngine;

public abstract class Item : ScriptableObject
{
    
    [SerializeField] protected int id;
    [SerializeField] protected ItemType type;
    [SerializeField] new protected string name = "New Item";
    [SerializeField] protected bool isDefaultItem = false;
    [SerializeField] protected int currencyValue;

    public ItemType Type => type;
    public int Id => id;
    public string Name => name;
    

    public Item Initialize(string _name, int _currencyValue, int itemId)
    {
        name = _name;
        currencyValue = _currencyValue;
        id = itemId;
        type = ItemType.Consumable;
        return this;
    }

    public virtual void Use(int _fromCID)
    {
        Debug.Log($"{NetworkManager.instance.Server.Clients[_fromCID].player.username} used item: {name}");
        Dispose(_fromCID);
    }

    public void Dispose(int _fromCID)
    {
        NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Inventory>().items.Remove(this);
    }
}
