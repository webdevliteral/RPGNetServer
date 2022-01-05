using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    private List<Quest> currentQuests = new List<Quest>();
    public List<Quest> CurrentQuests => currentQuests;
    private Player attachedPlayer;
    private void Awake()
    {
        attachedPlayer = GetComponent<Player>();
    }

    public void AddQuest(Quest quest)
    {
        quest.Initialize(attachedPlayer);
        currentQuests.Add(quest);
        
        ServerSend.UpdatePlayerQuestLog(attachedPlayer.id, quest);
    }

    public Quest FindQuestInQuestlog(int questId)
    {
        for(int i = 0; i< currentQuests.Count; i++)
        {
            if (currentQuests[i].id == questId)
                return currentQuests[i];
        }
        return null;
    }

    public bool IsFinished(Quest questToTrack)
    {
        if (questToTrack.isCompleted)
            return true;

        return false;
    }
}
