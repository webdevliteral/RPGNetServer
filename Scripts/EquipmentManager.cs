using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    public delegate void OnEquipmentChanged(RegularEquipment newItem, RegularEquipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    Inventory inventory;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    RegularEquipment[] equipped;
    void Start()
    {
        inventory = GetComponent<Inventory>();
        //returns a string array with all enum types
        int slots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        equipped = new RegularEquipment[slots];
    }

    public void Equip(RegularEquipment equipItem)
    {
        RegularEquipment oldItem = null;
        int slotIndex = (int)equipItem.equipSlot;

        if (equipped[slotIndex] != null)
        {
            oldItem = equipped[slotIndex];
            GetComponent<Inventory>().Add(oldItem);
        }

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(equipItem, oldItem);

        equipped[slotIndex] = equipItem;
    }

    public void Unequip(int slotIndex)
    {
        if (equipped[slotIndex] != null)
        {
            RegularEquipment oldItem = equipped[slotIndex];
            inventory.Add(oldItem);
            equipped[slotIndex] = null;
            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
    }

}


