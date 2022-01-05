using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAtlas : MonoBehaviour
{
    public static EntityAtlas instance;

    public List<Enemy> allEnemies = new List<Enemy>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public Enemy GetEnemyReferenceById(int id)
    {
        return allEnemies[id];
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
