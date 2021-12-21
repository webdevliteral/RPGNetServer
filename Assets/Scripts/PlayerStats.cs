using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EquipmentManager>().OnEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(RegularEquipment newItem, RegularEquipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
        }

    }
}
