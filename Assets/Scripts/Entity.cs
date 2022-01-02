using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkComponent))]
public abstract class Entity : MonoBehaviour, IInteractable
{
    protected NetworkComponent _networkComponent;
    public uint NetworkId => _networkComponent.NetworkId;

    [SerializeField]
    protected uint _entityPrefabId;

    public uint PrefabId => _entityPrefabId;

    [SerializeField]
    protected string _entityName;
    public string EntityName => _entityName;

    [SerializeField]
    private float interactRadius = 3f;
    public float InteractRadius => interactRadius;

    public float gravity = -9.18f;
    public float moveSpeed = 5f;

    protected bool hasInteracted = false;

    protected virtual void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;

        _networkComponent = GetComponent<NetworkComponent>();
        ClientRef.OnPlayerSpawned += OnPlayerSpawned;
    }

    public void Initialize(EntityAtlas.EntityData eData)
    {
        _entityName = eData.name;
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
        ServerSend.SpawnEntity(_networkComponent.NetworkId, _entityPrefabId, transform.position, transform.rotation);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
