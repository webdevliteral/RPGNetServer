using System;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public Item[] rewards;
    public int experience;
    public int currencyReward;
    [SerializeField] private List<Objective> objectives = new List<Objective>();
    public List<Objective> Objectives => objectives;
    private int tasksLeft;
    public bool isCompleted;

    public void Initialize(Player playerReference)
    {
        tasksLeft = objectives.Count;
        //foreach (KillObjective objective in Objectives)
        //{
        //    objective.Initialize();
        //    objective.OnObjectiveComplete += FinishObjective;
        //}
        foreach (CollectObjective objective in Objectives)
        {
            objective.Initialize(playerReference);
            objective.OnObjectiveComplete += FinishObjective;
        }
    }

    private void FinishObjective(Objective _objective)
    {
        if (objectives.Contains(_objective))
            _objective.isFinished = true;
        tasksLeft--;
        if (tasksLeft <= 0)
            isCompleted = true;
    }

    public void Initialize(int _id, string _title, string _description, Item[] _rewards, int _experience, int _currencyReward, List<Objective> _tasks)
    {
        id = _id;
        title = _title;
        description = _description;
        rewards = _rewards;
        experience = _experience;
        currencyReward = _currencyReward;
        objectives = _tasks;
    }
}
