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
    [SerializeField]
    private LayerMask _playerLayer; 
    public float gravity = -9.18f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    private float yVelocity = 0;
    public float lookRadius = 6f;
    protected float _stopDistance = 2.0f;
    protected Transform target;
    //Transform target;
    //NavMeshAgent agent;

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        moveSpeed *= Time.fixedDeltaTime;
    }

    protected void SearchForPlayersInsideRadius()
    {
        //Define what happens if you're in the search radius AND
        //you can draw a line from enemy to player
        Collider[] hits = Physics.OverlapSphere(transform.position, lookRadius, _playerLayer);
        foreach (Collider hit in hits)
        {
            target = hit.transform;
        }
    }


    protected void Move(Vector3 _direction, float _speed)
    {
        _direction.y = 0f;
        transform.forward = _direction;
        Vector3 _movement =transform.forward * _speed;

        if(controller.isGrounded)
        {
            yVelocity = 0f;
        }
        yVelocity += gravity;

        _movement.y = yVelocity;
        controller.Move(_movement);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
