using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellComponent : MonoBehaviour
{
    private Ability spell;
    private float cooldownTimer = 0f;

    public void InitializeSpellComponent(Ability ability)
    {
        spell = ability;
    }

    private void FixedUpdate()
    {
        cooldownTimer -= Time.deltaTime;
    }
  
    public void Use(int fromCID)
    {
        Focus playerFocus = NetworkManager.instance.Server.Clients[fromCID].player.focus;

        if (playerFocus.target != null)
        {
            Transform playerPosition = playerFocus.GetComponent<Transform>();
            Vector3 distance = playerPosition.position - playerFocus.target.position;

            if (distance.sqrMagnitude <= spell.MaxDistance * spell.MaxDistance)
            {
                Combat playerCombat = playerFocus.GetComponent<Combat>();
                Combat enemyCombat = playerFocus.target.GetComponent<Combat>();
                EnemyStats enemyStats = playerFocus.target.GetComponent<EnemyStats>();
                if (playerCombat != null && enemyCombat != null)
                {
                    if (cooldownTimer <= 0)
                    {
                        playerCombat.AttackWithSpell(enemyStats, spell.BaseDamage);
                        Debug.Log($"Cast {spell.Name}!");
                        cooldownTimer = spell.AbilityCooldown;
                    }

                }
            }
        }
    }
}
