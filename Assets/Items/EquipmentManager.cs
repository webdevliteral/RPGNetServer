using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private Inventory inventory;
    private RegularEquipment[] equipped;

    public event Action<RegularEquipment, RegularEquipment> OnEquipmentChanged;
    void Start()
    {
        inventory = GetComponent<Inventory>();
        int slots = Enum.GetNames(typeof(EquipmentSlot)).Length;
        equipped = new RegularEquipment[slots];
    }

    public void Equip(RegularEquipment equipItem, int fromCID, int id)
    {
        if(equipItem.Type == ItemType.SpellPiece)
            InitializeSpellPieceData(equipItem as SpellPiece);

        RegularEquipment oldItem = null;
        int slotIndex = (int)equipItem.equipSlot;

        if (equipped[slotIndex] != null)
        {
            oldItem = equipped[slotIndex];
            inventory.Add(oldItem);
        }
        

        OnEquipmentChanged?.Invoke(equipItem, oldItem);

        equipped[slotIndex] = equipItem;

        ServerSend.ItemEquipped(fromCID, id);
    }

    public void Unequip(int slotIndex)
    {
        if (equipped[slotIndex] != null)
        {
            RegularEquipment oldItem = equipped[slotIndex];
            inventory.Add(oldItem);
            equipped[slotIndex] = null;

            OnEquipmentChanged?.Invoke(null, oldItem);
        }
    }

    private void InitializeSpellPieceData(SpellPiece pieceToInit)
    {
        SpellComponent spellComponent = gameObject.AddComponent<SpellComponent>();
        spellComponent.InitializeSpellComponent(pieceToInit.SpellAbility);
        
        GetComponent<ActiveSpellbook>().AddSpellToActiveSpells(spellComponent, (int)pieceToInit.equipSlot);
    }
}


