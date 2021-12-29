using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class NotifyOnDestroy : MonoBehaviour
{
    //Create an event for when this is destroyed
    public event Action<AssetReference, NotifyOnDestroy> Destroyed;
    
    //Create a reference to our asset reference
    public AssetReference AssetReference { get; set; }

    public void OnDestroy()
    {
        //When our asset reference is destryoed, we invoke the Destroyed action
        //and notifying our asyncOperationHandlers or anything else that needs
        //to know
            
        Destroyed?.Invoke(AssetReference, this);
    }
}
