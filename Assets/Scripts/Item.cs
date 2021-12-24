using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int Id => id;
    [SerializeField] private int id;

    [SerializeField] new private string name = "New Item";
    [SerializeField] private Sprite icon = null;
    [SerializeField] private bool isDefaultItem = false;

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
