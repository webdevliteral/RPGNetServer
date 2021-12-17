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

    void FixedUpdate()
    {
        
    }

    public virtual bool Interact(int _fromCID, Vector3 _comparePosition)
    {

                Debug.Log($"{GameServer.clients[_fromCID].player.username} is focusing {transform.name}");
                float distance = Vector3.Distance(_comparePosition, transform.position);
                if (distance <= interactRadius)
                {
                    Debug.Log($"{GameServer.clients[_fromCID].player.username} is in range of {transform.position}");
                    return true;
                    //Interact(GameServer.clients[i].player.id);
                }
                else
                {
                    ServerSend.InteractableTooFar(GameServer.clients[_fromCID].CID);
                    Debug.Log($"Player {GameServer.clients[_fromCID].player.username} is trying to interact with: {transform.name} but is too far away!");
                    return false;
                }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}

