using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : NPC
{
    private EnemyStats myStats;

    public override void Start()
    {
        base.Start();
        myStats = GetComponent<EnemyStats>();
    }
    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if(base.Interact(_fromCID, _comparePosition))
        {
            Combat playerCombat = NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Combat>();

            if (playerCombat != null)
            {
                playerCombat.Attack(myStats);

                string _msg = $"Youre fighting {EntityName}";
                ServerSend.InteractionConfirmed(_fromCID, _msg);

                return true;
            }

            return false;
        }

        return false;
    }

}



