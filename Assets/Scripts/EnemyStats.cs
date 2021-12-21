using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStats : EntityStats
{
    [SerializeField]
    private List<GameObject> lootDropPrefabs = new List<GameObject>();
    public override void HandleDeath()
    {
        base.HandleDeath();
        //handle an enemy death
        //and loot drops
        EnemyManager.enemies.Remove(GetComponent<Enemy>().id);
        ServerSend.KillEnemy(gameObject);

        Destroy(gameObject);

        for(int i = 0; i < lootDropPrefabs.Count; i++)
        {
            var itemDrop = SpawnLoot(lootDropPrefabs[i]);
            ServerSend.SendClientsLootData(itemDrop.item.id, itemDrop.NetworkId, transform.position);
        }

    }

    private ItemDrop SpawnLoot(GameObject itemDropPrefab)
    {
        var itemDrop = Instantiate(itemDropPrefab, transform.position, Quaternion.identity).GetComponent<ItemDrop>();
        ItemManager.instance.AddItemDrop(itemDrop);
        return itemDrop;
    }
}