using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class AIController : MonoBehaviour
{
    CharacterController controller;
    public float gravity = -9.18f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    private float yVelocity = 0;
    public float lookRadius = 6f;
    Player target;
    //Transform target;
    //NavMeshAgent agent;

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if(target == null)
            SearchForPlayersInsideRadius();
        if (target != null)
        {
            Vector3 _direction = target.transform.position - transform.position;
            if (TargetInLineOfSight())
                Move(_direction, moveSpeed);
        }
    }

    private void SearchForPlayersInsideRadius()
    {
        foreach(ClientRef _client in GameServer.clients.Values)
        {
            if(_client.player != null)
            {
                Vector3 playerPos = _client.player.transform.position;
                Vector3 _direction = playerPos - transform.position;
                //float distance = Vector3.Distance(playerPos, transform.position);
                
                if (_direction.magnitude <= lookRadius)
                {
                    //Debug.Log("Player near enemy search radius!");
                    if (Physics.Raycast(transform.position, playerPos, out RaycastHit _hit, lookRadius))
                    {
                        //Define what happens if you're in the search radius AND
                        //you can draw a line from enemy to player
                        target = _hit.collider.GetComponent<Player>();  
                    }
                }
            }
        }
    }

    private bool TargetInLineOfSight()
    {
        if (target == null)
            return false;

        if(Physics.Raycast(transform.position, target.transform.position - transform.position, out RaycastHit _hit, lookRadius))
        {
            if(_hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }

    private void Move(Vector3 _direction, float _speed)
    {
        
        _direction.y = 0f;
        transform.forward = _direction;
        Vector3 _movement = transform.forward * _speed;

        if(controller.isGrounded)
        {
            yVelocity = 0f;
        }
        yVelocity += gravity;

        _movement.y = yVelocity;
        controller.Move(_movement);

        ServerSend.UpdateEnemyPosition(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
