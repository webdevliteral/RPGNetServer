using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int id;
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use(int _fromCID)
    {
        Debug.Log($"{GameServer.clients[_fromCID].player.username} used item: {name}");
        Dispose(_fromCID);
    }

    public void Dispose(int _fromCID)
    {
        GameServer.clients[_fromCID].player.GetComponent<Inventory>().Remove(this);
    }
}
