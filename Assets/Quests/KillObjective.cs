using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjective : Objective
{
    private Enemy goalEnemy;

    public void Initialize(int _numberToComplete, ObjectiveType _type, Enemy _goalEnemy)
    {
        numberToComplete = _numberToComplete;
        type = _type;
        goalEnemy = _goalEnemy;
    }
}
