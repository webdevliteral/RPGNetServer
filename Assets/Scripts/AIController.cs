using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkComponent))]
class AIController : MonoBehaviour
{
    private CharacterController controller;

    private NetworkComponent networkComponent;

    [SerializeField]
    protected LayerMask _playerLayer;

    private float _gravity = -9.81f;
    private float _yVelocity = 0;
    protected float _lookRadius = 6f;
    

    protected Transform target;

    protected virtual void Awake()
    {
        networkComponent = GetComponent<NetworkComponent>();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

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


    protected virtual void Move(Vector3 _direction, float _speed)
    {
        _direction.y = 0;
        //set the forward direction of our vector to our target direction
        transform.forward = _direction;

        //create a movement vector with our new direction, multiplied by the desired distance to move this by
        Vector3 _movement = transform.forward * _speed * Time.fixedDeltaTime;

        if(controller.isGrounded)
        {
            _yVelocity = 0f;
        }

        //apply gravity to this
        _yVelocity += _gravity;
        _movement.y = _yVelocity;

        controller.Move(_movement);
        ServerSend.UpdateEnemyPosition(networkComponent.NetworkId, transform.position);
    }

    void OnDrawGizmosSelected()
    {
        //set the gizmo color
        Gizmos.color = Color.red;
        //draw a sphere that color around the _lookRadius of this
        Gizmos.DrawWireSphere(transform.position, _lookRadius);
    }
}
