using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<Item> itemReferences = new List<Item>();

    struct ItemData
    {
        public string name;
        public string type;
        public int damageValue;
        public int armorValue;
        public int currencyValue;
        public string equipSlot;
        public int attachedSpellId;
        public int itemId;
        //TODO: retrieve AttachedAbilityId
        //and attach ability to any SpellPiece
    }

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItemReferencesOnServer()
    {
        for(int i = 0; i < itemReferences.Count; i++)
        {
            GameObject worldItem = Instantiate(itemPrefab);
            switch(itemReferences[i].Type)
            {
                case ItemType.RegularEquipment:
                    RegularEquipment equipToSpawn = itemReferences[i] as RegularEquipment;
                    worldItem.AddComponent<ItemDrop>().item = equipToSpawn;
                    break;
                case ItemType.SpellPiece:
                    SpellPiece spellPieceToSpawn = itemReferences[i] as SpellPiece;
                    worldItem.AddComponent<ItemDrop>().item = spellPieceToSpawn;
                    break;
            }
        }
    }

    public void CheckItemReferences()
    {
        Debug.Log("CURRENT ITEM DATABASE:");
        for(int i = 0; i < itemReferences.Count; i++)
        {
            Debug.Log($"Name: {itemReferences[i].Name} | ID: {itemReferences[i].Id}");
        }
    }

    public void RetrieveItemDataFromServer(int itemId)
    {
        string itemData = NetworkManager.instance.HTTPGet($"http://127.0.0.1:3100/item/{itemId}");
        if (itemData != "null")
        {
            ItemData newItemObj = JsonUtility.FromJson<ItemData>(itemData);
            switch(newItemObj.type)
            {
                case "Consumable":  
                    Debug.Log($"Loaded Consumable with id: {newItemObj.itemId}");
                    break;
                case "RegularEquipment":
                    RegularEquipment newEquipment = ScriptableObject.CreateInstance<RegularEquipment>();
                    newEquipment = newEquipment.Initialize(newItemObj.name, newItemObj.currencyValue, newItemObj.itemId,
                        newItemObj.damageValue, newItemObj.armorValue, newItemObj.equipSlot);
                    itemReferences.Add(newEquipment);
                    break;
                case "SpellPiece":
                    SpellPiece newSpellPiece = ScriptableObject.CreateInstance<SpellPiece>();
                    newSpellPiece = newSpellPiece.Initialize(newItemObj.name, newItemObj.currencyValue, newItemObj.itemId,
                        newItemObj.damageValue, newItemObj.armorValue, newItemObj.equipSlot, newItemObj.attachedSpellId);
                    itemReferences.Add(newSpellPiece);
                    break;
            }
            itemId++;
            RetrieveItemDataFromServer(itemId);            
        }
        else
        {
            Debug.Log("Retrieved all item data successfully!");
        }
    }
}

public enum ItemType { Consumable, RegularEquipment, SpellPiece }