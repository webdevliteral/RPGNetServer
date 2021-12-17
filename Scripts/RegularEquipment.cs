using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Regular Equipment", menuName = "Inventory/Equipment/New Regular Equipment")]
public class RegularEquipment : Item
{
    public EquipmentSlot equipSlot;
    public int armorModifier;
    public int damageModifier;

    public override void Use(int _fromCID)
    {
        base.Use(_fromCID);
        if(GameServer.clients[_fromCID].player.GetComponent<EquipmentManager>() != null)
        {
            GameServer.clients[_fromCID].player.GetComponent<EquipmentManager>().Equip(this);
            ServerSend.ItemEquipped(_fromCID, id);
            Dispose(_fromCID);
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
