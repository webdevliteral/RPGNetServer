using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collect Objective", menuName = "Objectives/New Collect Objective")]
public class CollectObjective : Objective
{
    [SerializeField] private ItemDrop goalItem;

    private void Awake()
    {
        type = ObjectiveType.Collect;
    }

    protected void UpdateObjective(Item item)
    {
        if (item == goalItem.item)
        {
            numberToComplete--;
            if (numberToComplete <= 0)
                InvokeObjectiveComplete();
            Debug.Log($"Finished collecting all {item.Name}s.");
        }
    }


    public void Initialize(Player player)
    {
        player.inventory.OnQuestItemLooted += UpdateObjective;
    }
}
