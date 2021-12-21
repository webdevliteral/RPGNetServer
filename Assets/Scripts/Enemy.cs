using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : NPC
{
    EnemyStats myStats;

    void Start()
    {
        myStats = GetComponent<EnemyStats>();
    }
    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (NetworkManager.instance.Server.Clients[_fromCID].player != null)
        {
            
            //Debug.Log($"Client {_fromCID} is trying to interact with an enemy from position {_comparePosition}");
            if (base.Interact(_fromCID, _comparePosition))
            {
                string _msg = $"You are now interacting with: {entityName}";

                Combat playerCombat = NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Combat>();

                if(playerCombat != null)
                {
                    playerCombat.Attack(myStats);
                    ServerSend.UpdateEnemyStats(gameObject);
                }
                    
                
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
}

