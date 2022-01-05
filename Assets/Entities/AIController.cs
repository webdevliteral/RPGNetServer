using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkComponent))]
public class AIController : MonoBehaviour
{
    private CharacterController controller;

    private NetworkComponent networkComponent;

    [SerializeField]
    protected LayerMask playerLayer;

    private float gravity = -9.81f;
    private float yVelocity = 0;
    protected float lookRadius = 6f;
    

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
        Collider[] hits = Physics.OverlapSphere(transform.position, lookRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            target = hit.transform;
            return;
        }
    }


    protected virtual void Move(Vector3 direction, float speed)
    {
        direction.y = 0;
        //set the forward direction of our vector to our target direction
        transform.forward = direction;

        //create a movement vector with our new direction, multiplied by the desired distance to move this by
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;

        if(controller.isGrounded)
        {
            yVelocity = 0f;
        }

        //apply gravity to this
        yVelocity += gravity;
        movement.y = yVelocity;

        controller.Move(movement);
        ServerSend.UpdateEnemyPosition(networkComponent.NetworkId, transform.position);
    }

    void OnDrawGizmosSelected()
    {
        //set the gizmo color
        Gizmos.color = Color.red;
        //draw a sphere that color around the _lookRadius of this
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
