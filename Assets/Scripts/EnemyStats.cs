using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStats : EntityStats
{
    //TODO: make this a list of items and instantiate with the itemPrefab
    [SerializeField]
    private List<GameObject> lootDropPrefabs = new List<GameObject>();

    public override void HandleDeath()
    {
        for(int i = 0; i < lootDropPrefabs.Count; i++)
        {
            GameObject lootDrop = lootDropPrefabs[i];

            SpawnLoot(lootDrop);
        }

        base.HandleDeath();

    }

    private ItemDrop SpawnLoot(GameObject itemDropPrefab)
    {
        return Instantiate(itemDropPrefab, transform.position, Quaternion.identity).GetComponent<ItemDrop>();
    }
}