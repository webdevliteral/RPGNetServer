using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class Stat
{
    [SerializeField]
    int baseValue;

    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        //int finalValue = baseValue;
        //modifiers.ForEach(x => finalValue += x);
        return baseValue;
    }

    public void SetValue(int _newValue)
    {
        baseValue = _newValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
    }
}
