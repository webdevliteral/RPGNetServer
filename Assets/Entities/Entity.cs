using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkComponent))]
public abstract class Entity : MonoBehaviour, IInteractable
{
    protected NetworkComponent networkComponent;
    

    [SerializeField]
    protected uint entityPrefabId;

    

    [SerializeField]
    protected string entityName;
    

    [SerializeField]
    private float interactRadius = 3f;

    public uint NetworkId => networkComponent.NetworkId;
    public uint PrefabId => entityPrefabId;
    public float InteractRadius => interactRadius;
    public string EntityName => entityName;

    public float gravity = -9.18f;
    public float moveSpeed = 5f;

    protected bool hasInteracted = false;

    protected virtual void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;

        networkComponent = GetComponent<NetworkComponent>();
        ClientRef.OnPlayerSpawned += OnPlayerSpawned;
    }

    public void Initialize(EntityAtlas.EntityData eData)
    {
        entityName = eData.name;
        interactRadius = eData.interactRadius;
    }


    public virtual bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        Vector3 distance = _comparePosition - transform.position;
        if(distance.sqrMagnitude <= interactRadius * interactRadius)
        {
            return true;
        }
        return false;
    }

    private void OnPlayerSpawned()
    {
        ServerSend.SpawnEntity(networkComponent.NetworkId, entityPrefabId, transform.position, transform.rotation);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
