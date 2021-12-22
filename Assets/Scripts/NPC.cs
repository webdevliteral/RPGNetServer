using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class NPC : Entity
{
    private Focus focus;

    
    public float gravity = -9.18f;
    public float moveSpeed = 5f;    

    protected virtual void Start()
    {
        focus = GetComponent<Focus>();
        _networkComponent = GetComponent<NetworkComponent>();
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;

        NetworkManager.instance.AddNetworkComponent(_networkComponent);
        ServerSend.EntityInfo(_networkComponent.NetworkId, _entityPrefabId, transform.position, transform.rotation);
    }

    public void SpawnInGame()
    {
        ServerSend.SpawnNPC(gameObject);
    }
}

