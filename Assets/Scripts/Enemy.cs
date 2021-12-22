using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : NPC
{
    private EnemyStats myStats;

    protected override void Start()
    {
        myStats = GetComponent<EnemyStats>();
        base.Start();
    }
    public override bool Interact(int _fromCID, Vector3 _comparePosition)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}



