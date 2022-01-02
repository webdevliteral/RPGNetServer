using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAtlas : MonoBehaviour
{
    public static EntityAtlas instance;

    private List<GameObject> allEntities = new List<GameObject>();
    public List<GameObject> AllEntities => allEntities;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void RetrieveEntityDataFromServer(int entityId)
    {
        string entityData = NetworkManager.instance.HTTPGet($"http://127.0.0.1:3100/entity/{entityId}");

        if (entityData != "null")
        {
            EntityData data = JsonUtility.FromJson<EntityData>(entityData);
            switch(data.entityType)
            {
                case "Enemy":
                    {
                        GameObject enemyObj = Instantiate(NetworkManager.instance.enemyPrefab, data.spawnPosition, Quaternion.identity);
                        enemyObj.name = data.name;
                        Enemy enemyData = enemyObj.GetComponent<Enemy>();

                        enemyData.Initialize(data);
                        allEntities.Add(enemyObj);
                        break;
                    }
                    
                case "NPC":
                    {
                        GameObject NPCObj = Instantiate(NetworkManager.instance.npcPrefab, data.spawnPosition, Quaternion.identity);

                        NPCObj.name = data.name;
                        NPC npcData = NPCObj.GetComponent<NPC>();

                        npcData.Initialize(data);
                        allEntities.Add(NPCObj);
                        break;
                    }
                    
            }

            entityId++;
            RetrieveEntityDataFromServer(entityId);
        }

    }

    public struct EntityData
    {
        public int id;
        public string name;
        public int interactRadius;
        public string entityType;
        public Vector3 spawnPosition;
    }
}
