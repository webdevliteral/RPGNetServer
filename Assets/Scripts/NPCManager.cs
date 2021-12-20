using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class NPCManager : MonoBehaviour
{
    public static NPCManager instance;
    public static Dictionary<int, GameObject> npcList = new Dictionary<int, GameObject>();

    public GameObject npcPrefab;
    public float npcRespawnCooldown = 20.0f;
    private float respawnTimer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void FixedUpdate()
    {
        if (respawnTimer <= 0)
        {
            //RespawnEnemies();
            respawnTimer = npcRespawnCooldown;
        }
        if (respawnTimer > 0)
            respawnTimer -= Time.deltaTime;

    }
    public void InitNPCData()
    {
        /////////////////////////////////////////////////////////
        //init the enemies in the scene

        //Define an empty enemy prefab
        GameObject npc = NetworkManager.instance.InstantiateNPC(-3.2f, 1.0f, 1.1f);

        //Define that enemy's traits
        npc.GetComponent<NPC>().id = 0;
        npc.GetComponent<NPC>().entityName = "Tutorial Guide";
        npc.GetComponent<NPC>().interactRadius = 5f;

        //finally add the enemy to the list of enemies to be spawned on the server
        npcList.Add(npc.GetComponent<NPC>().id, npc);

        ////////////////////////////////////////////////////////


        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<NPC>().SpawnInGame();
        }


    }

    public void LoadNPCListOnClient(int _fromClient)
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            if (npcList[i] != null)
                ServerSend.SendNPCData(_fromClient, npcList[i]);
        }

    }
}
