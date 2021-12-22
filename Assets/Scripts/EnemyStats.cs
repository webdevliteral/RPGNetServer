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

        for(int i = 0; i < lootDropPrefabs.Count; i++)
        {
            int prefabId = (int)lootDropPrefabs[i].GetComponent<Entity>().PrefabId;
            SpawnLoot(NetworkManager.instance.prefabs[prefabId]);
        }

    }

    private ItemDrop SpawnLoot(GameObject itemDropPrefab)
    {
        var itemDrop = Instantiate(itemDropPrefab, transform.position, Quaternion.identity).GetComponent<ItemDrop>();
        return itemDrop;
    }
}