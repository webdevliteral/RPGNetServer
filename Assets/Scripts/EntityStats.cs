using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class EntityStats : MonoBehaviour
{
    private NetworkComponent _networkComponent;

    public int maxHealth = 100;
    public int currentHealth;
    public Stat damage;
    public Stat armor;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        _networkComponent = GetComponent<NetworkComponent>();
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
            ServerSend.UpdateEntityStats(_networkComponent.NetworkId, currentHealth);
            return;
        }
        ServerSend.UpdateEntityStats(_networkComponent.NetworkId, currentHealth);
    }

    public virtual void HandleDeath()
    {
        Destroy(gameObject);
    }
}
