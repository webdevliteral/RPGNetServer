public class PlayerStats : EntityStats
{
    protected override void Start()
    {
        base.Start();
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
