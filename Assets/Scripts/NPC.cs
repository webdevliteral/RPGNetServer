using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NPC : Interactable
{
    public string entityName;
    public float gravity = -9.18f;
    public float moveSpeed = 5f;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id)
    {
        id = _id;
    }

    public override bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        if (NetworkManager.instance.Server.Clients[_fromCID].player != null)
        {

            //Debug.Log($"Client {_fromCID} is trying to interact with an enemy from position {_comparePosition}");
            if (base.Interact(_fromCID, _comparePosition))
            {
                string _msg = $"You are now interacting with: {GetComponent<NPC>().entityName}";

                ServerSend.InteractionConfirmed(_fromCID, GetComponent<NPC>().id, _msg);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    public void SpawnInGame()
    {
            ServerSend.SpawnNPC(gameObject);
    }
}

