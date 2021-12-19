using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStats : EntityStats
{
    public List<GameObject> lootDrops = new List<GameObject>();
    public override void HandleDeath()
    {
        base.HandleDeath();
        //handle an enemy death
        //and loot drops
        EnemyManager.enemies.Remove(GetComponent<Enemy>().id);
        ServerSend.KillEnemy(gameObject);

        Destroy(gameObject);

        for(int i = 0; i < lootDrops.Count; i++)
        {
            int _setID = ItemManager.instance.localItems.Count;
            
            lootDrops[i].transform.position = transform.position;
            SpawnLoot(lootDrops[i], _setID);
            ServerSend.SendClientsLootData(lootDrops[i].GetComponent<ItemDrop>().item.id, _setID, transform.position);
        }

    }

    public void SpawnLoot(GameObject _item, int _setID)
    {
        GameObject copy = Instantiate(_item, transform.position, Quaternion.identity);
        copy.GetComponent<ItemDrop>().id = _setID;
        ItemManager.instance.localItems.Add(copy);
        
    }
}