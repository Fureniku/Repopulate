using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase : MonoBehaviour {
    
    [SerializeField] private EnumResource resourceType;
    
    private void Start() {
        ResourceEventManager.OnResourceObjectCreated(resourceType, this);
        ResourceStart();
    }

    private void OnDestroy() {
        ResourceEventManager.OnResourceObjectDestroyed(resourceType, this);
        ResourceDestroy();
    }

    protected void ResourceStart() { }
    protected void ResourceDestroy() { }
}
