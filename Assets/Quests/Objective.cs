using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    protected int numberToComplete;
    protected ObjectiveType type;
}

public enum ObjectiveType
{
    Collect,
    Kill,
    Solve,
    Discover
}