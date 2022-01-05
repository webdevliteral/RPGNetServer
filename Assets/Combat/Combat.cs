using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
class Combat : MonoBehaviour
{
    private float attackSpeed = 1.0f;
    private float attackCooldown = 0f;
    private float castSpeed;

    private EntityStats myStats;

    private void Start()
    {
        myStats = GetComponent<EntityStats>();
    }

    private void Update()
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

    public void AttackWithSpell(EntityStats targetStats, int baseDamage)
    {
        float baseModifier = myStats.damage.GetValue();
        baseModifier *= 0.25f;
        float finalDamage = baseDamage * baseModifier;
        targetStats.TakeDamage((int)finalDamage);
    }
}
