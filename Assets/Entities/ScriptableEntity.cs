using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEntity : ScriptableObject
{
    private int id;
    public int Id => id;
    new private string name;
    public string Name => name;
    private float interactRadius;
    public float InteractRadius => interactRadius;
    private EntityType type;
    public EntityType Type => type;

    private int[] containedQuests;

    public void Initialize(int _id, string _name, float _interactRadius, EntityType _type)
    {
        id = _id;
        name = _name;
        interactRadius = _interactRadius;
        type = _type;
    }

    public int[] GetQuestsFromEntityData()
    {
        return containedQuests;
    }
}

public enum EntityType
{
    NPC,
    ENEMY
}