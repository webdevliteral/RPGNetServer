using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    [SerializeField]protected int numberToComplete;
    protected ObjectiveType type;
    public event Action<Objective> OnObjectiveComplete;
    public bool isFinished;

    protected void UpdateObjective()
    {
        numberToComplete--;
        if (numberToComplete <= 0)
            InvokeObjectiveComplete();
        Debug.Log($"Finished {type} objective.");
    }

    protected void InvokeObjectiveComplete()
    {
        OnObjectiveComplete?.Invoke(this);
    }

}

public enum ObjectiveType
{
    Collect,
    Kill,
    Solve,
    Discover
}