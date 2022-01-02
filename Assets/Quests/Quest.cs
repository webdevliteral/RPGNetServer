using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/New Quest")]
public class Quest : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public Item[] rewards;
    public int experience;
    public int currencyReward;
    public List<int> interactionIds = new List<int>();
    public ObjectiveType objectiveType;
    public Objective[] questObjectives;

    public void Initialize(int _id, string _title, string _description, Item[] _rewards, int _experience, int _currencyReward, Objective[] _tasks, int _objectiveType, int _interactId)
    {
        id = _id;
        title = _title;
        description = _description;
        rewards = _rewards;
        experience = _experience;
        currencyReward = _currencyReward;
        questObjectives = _tasks;
        interactionIds.Add(_interactId);
        objectiveType = (ObjectiveType)_objectiveType;
    }
}
