using UnityEngine;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Combat))]
public class Enemy : Entity
{
    private EnemyStats myStats;

    private Combat playerCombat;
    protected override void Start()
    {
        base.Start();
        myStats = GetComponent<EnemyStats>();
        EntityAtlas.instance.allEnemies.Add(this);
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if(base.Interact(_fromCID, _comparePosition))
        {
            if(playerCombat == null)
                playerCombat = NetworkManager.instance.Server.Clients[_fromCID].player.GetComponent<Combat>();

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



