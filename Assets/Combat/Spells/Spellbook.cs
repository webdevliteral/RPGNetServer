using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spellbook : MonoBehaviour
{
    public static Spellbook instance;
    [SerializeField]
    private List<Ability> spellBook = new List<Ability>();

    private void Awake()
    {
        instance = this;
    }

    struct SpellData
    {
        public string name;
        public float maxCastDistance;
        public int baseDamage;
        public float cooldownLength;
        public float castTime;
        public int id;
    }

    public Ability FindSpellById(int id)
    {
        if (spellBook[id] != null)
            return spellBook[id];
        return null;
    }

    public void RetrieveSpellDataFromServer(int spellId)
    {
        string spellData = NetworkManager.HTTP.GET($"http://127.0.0.1:3100/spell/{spellId}");
        if (spellData != "null")
        {
            SpellData newSpell = JsonUtility.FromJson<SpellData>(spellData);
            string savePath = $"Assets/Combat/Spells/SpellData/{newSpell.name}.asset";

            Ability newSpellAbility = ScriptableObject.CreateInstance<Ability>();
            newSpellAbility.InitializeAbility(newSpell.id, newSpell.name, newSpell.maxCastDistance,
                newSpell.baseDamage, newSpell.cooldownLength, newSpell.castTime);

            if (!System.IO.File.Exists(savePath))
                AssetDatabase.CreateAsset(newSpellAbility, savePath);

            //TODO: this only works in the editor
            Ability finalAbility = (Ability)AssetDatabase.LoadAssetAtPath(savePath, typeof(Ability));

            spellBook.Add(finalAbility);

            spellId++;
            RetrieveSpellDataFromServer(spellId);
        }
        else
        {
            Debug.Log("Retrieved all spell data successfully!");
        }
    }
}
