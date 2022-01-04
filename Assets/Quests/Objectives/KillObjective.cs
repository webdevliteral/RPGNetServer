
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Objective", menuName = "Objectives/New Kill Objective")]
public class KillObjective : Objective
{
    [SerializeField]private int goalEnemyId;
    
    private void Awake()
    {
        type = ObjectiveType.Kill;
    }

    public void Initialize()
    {
        Enemy goalEnemy = EntityAtlas.instance.GetEnemyReferenceById(goalEnemyId);
        goalEnemy.GetComponent<EntityStats>().OnDeath += UpdateObjective;
    }

    
}
