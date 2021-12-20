using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
class Combat : MonoBehaviour
{
    public float attackSpeed = 1.0f;
    private float attackCooldown = 0f;
    public float castSpeed;
    EntityStats myStats;

    void Start()
    {
        myStats = GetComponent<EntityStats>();
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    public void Attack(EntityStats targetStats)
    {
        if(attackCooldown <= 0f)
        {
            targetStats.TakeDamage(myStats.damage.GetValue());
            attackCooldown = 1f / attackSpeed;
        }
    }
}
