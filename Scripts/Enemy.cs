using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : NPC
{
    public Enemy(int _id, string _name)
    {
        id = _id;
        entityName = _name;
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (GameServer.clients[_fromCID].player != null)
        {
            
            //Debug.Log($"Client {_fromCID} is trying to interact with an enemy from position {_comparePosition}");
            if (base.Interact(_fromCID, _comparePosition))
            {
                string _msg = $"You are now interacting with: {GetComponent<Enemy>().entityName}";

                GetComponent<EnemyStats>().TakeDamage(_fromCID, 20);                    
                ServerSend.InteractionConfirmed(_fromCID, GetComponent<Enemy>().id, _msg);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
            
    }
    public void SpawnInGame()
    {
        if(this != null)
        {
            ServerSend.SpawnEnemy(gameObject);
        }
    }
}

