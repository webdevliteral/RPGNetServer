using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    new public QuestItem Initialize(string _name, int _currencyValue, int itemId)
    {
        name = _name;
        currencyValue = _currencyValue;
        id = itemId;
        type = ItemType.QuestItem;
        return this;
    }
}
