using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpellbook : MonoBehaviour
{
    [SerializeField]
    private List<SpellComponent> spells = new List<SpellComponent>();
    public List<SpellComponent> Spells => spells;
    private void Awake()
    {
        for(int i = 0; i < Enum.GetNames(typeof(EquipmentSlot)).Length; i++)
        {
            spells.Add(null);
        }
    }
    public void AddSpellToActiveSpells(SpellComponent spellToAdd, int activeSpellIndex)
    {
        spells[activeSpellIndex] = spellToAdd;
    }

}
