using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{
    public int Id => id;
    [SerializeField] protected int id;
    
    [SerializeField] protected ItemType type;
    public ItemType Type => type;

    [SerializeField] new protected string name = "New Item";
    public string Name => name;
    [SerializeField] protected bool isDefaultItem = false;
    [SerializeField] protected int currencyValue;

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
