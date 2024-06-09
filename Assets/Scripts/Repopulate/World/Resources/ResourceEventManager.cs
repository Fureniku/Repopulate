using System;
using UnityEngine;

public static class ResourceEventManager {
    public static event Action<EnumResource, object> ResourceObjectCreated;
    public static event Action<EnumResource, object> ResourceObjectDestroyed;

    public static void OnResourceObjectCreated(EnumResource type, object energyObject) {
        ResourceObjectCreated?.Invoke(type, energyObject);
    }

    public static void OnResourceObjectDestroyed(EnumResource type, object energyObject) {
        ResourceObjectDestroyed?.Invoke(type, energyObject);
    }
}