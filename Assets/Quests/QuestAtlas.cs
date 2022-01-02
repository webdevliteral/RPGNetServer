using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestAtlas : MonoBehaviour
{
    public static QuestAtlas instance;
    [SerializeField]
    private List<Quest> allQuests = new List<Quest>();
    public List<Quest> AllQuests => allQuests;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void RetrieveQuestDataFromServer(int questId)
    {
        string questData = NetworkManager.instance.HTTPGet($"http://127.0.0.1:3100/quest/{questId}");
        
        if(questData != "null")
        {
            QuestData data = JsonUtility.FromJson<QuestData>(questData);
            Quest newQuest = ScriptableObject.CreateInstance<Quest>();
            newQuest.Initialize(data.id, data.title, data.description, data.rewards, data.experience, data.currency, data.objectives, data.objective, data.collectionId);
            allQuests.Add(newQuest);

            string savePath = $"Assets/Quests/QuestData/{data.title}.asset";

            if (!System.IO.File.Exists(savePath))
                AssetDatabase.CreateAsset(newQuest, savePath);

            Quest finalQuest = AssetDatabase.LoadAssetAtPath<Quest>(savePath);

            allQuests.Add(finalQuest);
            questId++;
            RetrieveQuestDataFromServer(questId);
        }
        
    }

    public struct QuestData
    {
        public int id;
        public string title;
        public string description;
        public Item[] rewards;
        public int experience;
        public int currency;
        public int objective;
        public int collectionId;
        public Objective[] objectives;
        public int npcIdToAttach;
    }
}
