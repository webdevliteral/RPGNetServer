using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public static Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();

    public GameObject enemyPrefab;
    public float enemyRespawnCooldown = 20.0f;
    private float respawnTimer;
    //public static int fakeEnemies = 10;

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
            respawnTimer = enemyRespawnCooldown;
        }
        if(respawnTimer > 0)
            respawnTimer -= Time.deltaTime;
        
    }
    public void InitEnemyData()
    {
        /////////////////////////////////////////////////////////
        //init the enemies in the scene

        //Define an empty enemy prefab
        GameObject enemy = NetworkManager.instance.InstantiateEnemy(0.16f, -5.3f, 25.0f);
        
        //Define that enemy's traits
        enemy.GetComponent<Enemy>().id = 0;
        enemy.GetComponent<Enemy>().entityName = "DaddiChz";
        enemy.GetComponent<Enemy>().interactRadius = 5f;

        //finally add the enemy to the list of enemies to be spawned on the server
        enemies.Add(0, enemy);

        ////////////////////////////////////////////////////////
        
        // A couple more enemies just for testing
            // TODO: create a method for each enemy that returns an enemy
            // gObject which can be added to the list

        GameObject difEnemy = NetworkManager.instance.InstantiateEnemy(0.16f, -5.3f, 44.7f);
        difEnemy.GetComponent<Enemy>().id = 1;
        difEnemy.GetComponent<Enemy>().entityName = "SauceMama";
        difEnemy.GetComponent<Enemy>().interactRadius = 5f;


        enemies.Add(1, difEnemy);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().SpawnInGame();
        }

        
    }

    public void LoadEnemiesOnClient(int _fromClient)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] != null)
                ServerSend.SendEnemyData(_fromClient, enemies[i]);
        }
        
    }
}

