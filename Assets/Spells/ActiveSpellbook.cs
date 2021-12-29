using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpellbook : MonoBehaviour
{
    [SerializeField]
    private List<SpellComponent> spells = new List<SpellComponent>();
    public List<SpellComponent> Spells => spells;
    private void Awake()
    {
        //TODO: reference this from somewhere instead of hardcoding
            //Equipment manager probably??111?
        // 11 is the total number of possible equip slots, therefore
        //equipped abilities

        for(int i = 0; i < 11; i++)
        {
            spells.Add(null);
        }
    }
    public void AddSpellToActiveSpells(SpellComponent spellToAdd, int activeSpellIndex)
    {
        spells[activeSpellIndex] = spellToAdd;
    }

}
