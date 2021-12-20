using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Stat damage;
    public Stat armor;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        
    }
    public void TakeDamage(int _damage)
    {
        _damage -= armor.GetValue();
        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);

        currentHealth -= _damage;
        Debug.Log($"{transform.name} took {_damage} damage and now has {currentHealth} health. {armor.GetValue()} armor affected this hit.");
        if (currentHealth <= 0)
        {
            HandleDeath();
            return;
        }
    }

    public virtual void HandleDeath()
    {
        //TODO: handle death
        currentHealth = maxHealth;
    }
}
