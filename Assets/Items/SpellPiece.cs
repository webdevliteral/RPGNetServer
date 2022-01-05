using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Piece", menuName = "Inventory/Equipment/New Spell Piece")]
public class SpellPiece : RegularEquipment
{
    [SerializeField]private Ability spellAbility;
    public Ability SpellAbility => spellAbility;

    public SpellPiece Initialize(string _name, int _currencyValue, int itemId,
        int damageValue, int armorValue, string _equipSlot, int attachedSpellId)
    {
        name = _name;
        currencyValue = _currencyValue;
        id = itemId;
        armorModifier = armorValue;
        damageModifier = damageValue;
        type = ItemType.SpellPiece;
        spellAbility = Spellbook.instance.FindSpellById(attachedSpellId);
        switch (_equipSlot)
        {
            case "Head":
                equipSlot = EquipmentSlot.Head;
                break;
            case "Neck":
                equipSlot = EquipmentSlot.Neck;
                break;
            case "Shoulder":
                equipSlot = EquipmentSlot.Shoulder;
                break;
            case "Back":
                equipSlot = EquipmentSlot.Back;
                break;
            case "Chest":
                equipSlot = EquipmentSlot.Chest;
                break;
            case "Waist":
                equipSlot = EquipmentSlot.Waist;
                break;
            case "Legs":
                equipSlot = EquipmentSlot.Legs;
                break;
            case "Feet":
                equipSlot = EquipmentSlot.Feet;
                break;
            case "Trinket":
                equipSlot = EquipmentSlot.Trinket;
                break;
            case "MainHand":
                equipSlot = EquipmentSlot.MainHand;
                break;
            case "OffHand":
                equipSlot = EquipmentSlot.OffHand;
                break;
        }
        return this;
    }


}
