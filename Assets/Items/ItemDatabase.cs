using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public void RetrieveItemDataFromServer(int itemId)
    {
        string itemData = NetworkManager.HTTP.GET($"http://127.0.0.1:3100/item/{itemId}");
        if (itemData != "null")
        {
            ItemData newItemObj = JsonUtility.FromJson<ItemData>(itemData);
            string savePath = $"Assets/Items/ItemDB/{newItemObj.name}.asset";


            switch (newItemObj.type)
            {
                case "Consumable":
                    Debug.Log($"Loaded Consumable with id: {newItemObj.itemId}");
                    break;
                case "RegularEquipment":
                    RegularEquipment newEquipment = ScriptableObject.CreateInstance<RegularEquipment>();
                    newEquipment = newEquipment.Initialize(newItemObj.name, newItemObj.currencyValue, newItemObj.itemId,
                        newItemObj.damageValue, newItemObj.armorValue, newItemObj.equipSlot);

                    if (!System.IO.File.Exists(savePath))
                        AssetDatabase.CreateAsset(newEquipment, savePath);

                    RegularEquipment finalEquipment = AssetDatabase.LoadAssetAtPath<RegularEquipment>(savePath);
                    itemReferences.Add(finalEquipment);
                    break;
                case "SpellPiece":
                    SpellPiece newSpellPiece = ScriptableObject.CreateInstance<SpellPiece>();
                    newSpellPiece = newSpellPiece.Initialize(newItemObj.name, newItemObj.currencyValue, newItemObj.itemId,
                        newItemObj.damageValue, newItemObj.armorValue, newItemObj.equipSlot, newItemObj.attachedSpellId);

                    if (!System.IO.File.Exists(savePath))
                        AssetDatabase.CreateAsset(newSpellPiece, savePath);

                    SpellPiece finalSpellPiece = AssetDatabase.LoadAssetAtPath<SpellPiece>(savePath);
                    itemReferences.Add(finalSpellPiece);
                    break;
                case "QuestItem":
                    QuestItem newQuestItem = ScriptableObject.CreateInstance<QuestItem>();
                    newQuestItem.Initialize(newItemObj.name, newItemObj.currencyValue, newItemObj.itemId);

                    if (!System.IO.File.Exists(savePath))
                        AssetDatabase.CreateAsset(newQuestItem, savePath);

                    QuestItem finalQuestItem = AssetDatabase.LoadAssetAtPath<QuestItem>(savePath);
                    itemReferences.Add(finalQuestItem);
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

    public Item FindItemById(int id)
    {
        if (itemReferences[id] != null)
            return itemReferences[id];
        return null;
    }
}

public enum ItemType { Consumable, RegularEquipment, SpellPiece, QuestItem}