using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int id;
    public bool isFocused;
    public bool hasInteracted = false;
    public float interactRadius = 3f;
    public InteractionType interactionType;

    void FixedUpdate()
    {
        
    }

    public virtual bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        Debug.Log($"{NetworkManager.instance.Server.Clients[_fromCID].player.username} is focusing {transform.name}");

        float distance = (_comparePosition - transform.position).sqrMagnitude;

        if (distance <= interactRadius * interactRadius && hasInteracted == false)
        {
            Debug.Log($"{NetworkManager.instance.Server.Clients[_fromCID].player.username} is in range of {transform.position}");
            return true;
        }
        else
        {
            ServerSend.InteractableTooFar(NetworkManager.instance.Server.Clients[_fromCID].CID);
            Debug.Log($"Player {NetworkManager.instance.Server.Clients[_fromCID].player.username} is trying to interact with: {transform.name} but is too far away!");
            return false;
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}

public enum InteractionType
{
    Item,
    Enemy,
    NPC,
    Teleport,
    Player
}
