using UnityEngine;

[RequireComponent(typeof(CharacterController))]
class AIController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    protected LayerMask _playerLayer;

    protected float _gravity = -9.81f;
    protected float _moveSpeed = 5f;
    protected float _yVelocity = 0;
    protected float _lookRadius = 6f;
    protected float _stopDistance = 2.0f;

    protected Transform target;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //multiply movespeed by framerate to be interpreted as "distance over time"
        _moveSpeed *= Time.fixedDeltaTime;
    }

    protected void SearchForPlayersInsideRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _lookRadius, _playerLayer);

        foreach (Collider hit in hits)
        {
            target = hit.transform;
            return;
        }
    }


    protected void Move(Vector3 _direction, float _speed)
    {
        //set the forward direction of our vector to our target direction
        transform.forward = _direction;

        //create a movement vector with our new direction, multiplied by the desired speed to move this by
        Vector3 _movement = transform.forward * _speed;

        if(controller.isGrounded)
        {
            _yVelocity = 0f;
        }

        //apply gravity to this
        _yVelocity += _gravity;
        _movement.y = _yVelocity;

        controller.Move(_movement);
    }

    void OnDrawGizmosSelected()
    {
        //set the gizmo color
        Gizmos.color = Color.red;
        //draw a sphere that color around the _lookRadius of this
        Gizmos.DrawWireSphere(transform.position, _lookRadius);
    }
}
