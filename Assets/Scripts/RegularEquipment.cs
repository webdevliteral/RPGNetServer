using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Regular Equipment", menuName = "Inventory/Equipment/New Regular Equipment")]
public class RegularEquipment : Item
{
    public EquipmentSlot equipSlot;
    public int armorModifier;
    public int damageModifier;

    public RegularEquipment Initialize(string _name, int _currencyValue, int itemId,
        int damageValue, int armorValue, string _equipSlot)
    {
        name = _name;
        currencyValue = _currencyValue;
        id = itemId;
        armorModifier = armorValue;
        damageModifier = damageValue;
        type = ItemType.RegularEquipment;
        switch(_equipSlot)
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

    public override void Use(int _fromCID)
    {
        base.Use(_fromCID);
        if (NetworkManager.instance.Server.Clients[_fromCID].player.equipmentManager != null)
        {
            NetworkManager.instance.Server.Clients[_fromCID].player.equipmentManager.Equip(this, _fromCID, Id);
        }
    }
}

public enum EquipmentSlot
{
    Head,
    Neck,
    Shoulder,
    Back,
    Chest,
    Waist,
    Legs,
    Feet,
    Trinket,
    MainHand,
    OffHand
}
