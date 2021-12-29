using System.Collections.Generic;
using UnityEngine;

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
        string spellData = NetworkManager.instance.HTTPGet($"http://127.0.0.1:3100/spell/{spellId}");
        if (spellData != "null")
        {
            SpellData newSpell = JsonUtility.FromJson<SpellData>(spellData);
            var newSpellAbility = new Ability();
            newSpellAbility.InitializeAbility(newSpell.id, newSpell.name, newSpell.maxCastDistance, 
                newSpell.baseDamage, newSpell.cooldownLength, newSpell.castTime);
            spellBook.Add(newSpellAbility);            
            spellId++;
            RetrieveSpellDataFromServer(spellId);
        }
        else
        {
            Debug.Log("Retrieved all spell data successfully!");
        }
    }
}
