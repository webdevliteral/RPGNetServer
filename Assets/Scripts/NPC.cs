using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class NPC : Entity
{
    [SerializeField]
    private List<Quest> assignableQuests = new List<Quest>();

    [SerializeField]
    private string[] dialogue;
    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if(base.Interact(_fromCID, _comparePosition))
        {
            if (ServerSend.SendQuestToClient(_fromCID, assignableQuests[0]))
                return true;
            ServerSend.UseNPCDialogue(_fromCID, NetworkId, dialogue[0]);
        }
            
        
        return true;
    }
}

