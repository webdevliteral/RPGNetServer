using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class ItemDrop : Entity
{
    public Item item;
    public event Action OnItemLooted;

    private void Awake()
    {
        networkComponent = GetComponent<NetworkComponent>();
    }

    new private void Start()
    {
        ServerSend.SpawnEntity(NetworkId, PrefabId, transform.position, Quaternion.identity);
        ClientRef.OnPlayerSpawned += OnPlayerSpawned;
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        Debug.Log($"Tryin' to loot item: {item.name}");

        bool pickedUp = NetworkManager.instance.Server.Clients[_fromCID].player.inventory.Add(item);

        if (pickedUp)
        {
            if(NetworkManager.instance.RemoveNetworkComponent(networkComponent))
            {
                Debug.Log($"Removed {item.Name} from world");
                OnItemLooted?.Invoke();
                Destroy(gameObject);
                ServerSend.ItemLooted(_fromCID, NetworkId, item.Id);
            }

            return true;
        }

        return false;
    }

    private void OnPlayerSpawned()
    {
        ServerSend.SpawnEntity(NetworkId, PrefabId, transform.position, Quaternion.identity);
    }
}
