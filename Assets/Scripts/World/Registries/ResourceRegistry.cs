using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResourceRegistry : ScriptableObject {
    
    private static ResourceRegistry _instance;
    
    public static ResourceRegistry Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<ResourceRegistry>("Resource_Registry");
                if (_instance == null) {
                    Debug.LogError("No instance of Item_Registry found in Resources folder.");
                    return null;
                }

                _instance.ResourceList = _instance.GetResourceList();
                _instance.ResourceCount = _instance.ResourceList.Count;
            }
            return _instance;
        }
    }

    public List<Resource> ResourceList { get; private set; }
    public int ResourceCount { get; private set; }

    public Resource EMPTY;
    public Resource RAW_IRON_ORE;
    public Resource RAW_GOLD_ORE;
    public Resource RAW_CARBON_ORE;
    public Resource RAW_COPPER_ORE;
    public Resource RAW_URANIUM_ORE;

    private List<Resource> GetResourceList() {
        List<Resource> resourceList = new();
        
        //These MUST be in ID order.
        resourceList.Add(EMPTY);
        resourceList.Add(RAW_IRON_ORE);
        resourceList.Add(RAW_GOLD_ORE);
        resourceList.Add(RAW_CARBON_ORE);
        resourceList.Add(RAW_COPPER_ORE);
        resourceList.Add(RAW_URANIUM_ORE);
        
        return resourceList;
    }

    public Resource GetFromID(int id) {
        return id < ResourceList.Count ? ResourceList[id] : null;
    }

    public Resource GetFromName(string resourceName) {
        for (int i = 0; i < ResourceCount; i++) {
            if (String.CompareOrdinal(ResourceList[i].UnlocalizedName, resourceName) == 0) {
                return ResourceList[i];
            }
        }
        return null;
    }
}
