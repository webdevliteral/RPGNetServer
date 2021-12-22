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
        Debug.Log(NetworkManager.instance.Server.Clients[_fromCID].player);
        if (NetworkManager.instance.Server.Clients[_fromCID].player.equipmentManager != null)
        {
            NetworkManager.instance.Server.Clients[_fromCID].player.equipmentManager.Equip(this);
            ServerSend.ItemEquipped(_fromCID, id);
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
