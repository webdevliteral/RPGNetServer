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
            GameObject itemDropGameObject = SpawnLoot(lootDropPrefabs[i]);

            // TODO: FIX CAST
            ServerSend.SendClientsLootData(lootDropPrefabs[i].GetComponent<ItemDrop>().item.id, (int)itemDropGameObject.GetComponent<NetworkComponent>().NetworkId, transform.position);
        }

    }

    private GameObject SpawnLoot(GameObject _item)
    {
        GameObject itemDropGameObject = Instantiate(_item, transform.position, Quaternion.identity);
        ItemManager.instance.AddItemDrop(itemDropGameObject.GetComponent<NetworkComponent>());
        return itemDropGameObject;
    }
}