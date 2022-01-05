using System;
using UnityEngine;

public class NetworkComponent : MonoBehaviour
{
    private static uint networkIdGenerator = 1;

    /// <summary>
    /// An identifier that is unique per object, and synced across the server and all game clients.
    /// A network id can never be 0.
    /// </summary>
    public uint NetworkId => networkId;
    private uint networkId;

    protected virtual void Awake()
    {
        networkId = networkIdGenerator++;
    }

    private void Start()
    {
        NetworkManager.instance.AddNetworkComponent(this);
    }
}
