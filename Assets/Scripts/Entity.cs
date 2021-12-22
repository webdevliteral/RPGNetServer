using UnityEngine;

[DisallowMultipleComponent]
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
    protected float interactRadius = 3f;
    public float InteractRadius => interactRadius;

    protected bool hasInteracted = false;

    public virtual bool Interact(int _fromCID, Vector3 _comparePosition)
    {
        string _msg = $"You are now interacting with: {_entityName}";
        ServerSend.InteractionConfirmed(_fromCID, _msg);
        return true;
    }
}
