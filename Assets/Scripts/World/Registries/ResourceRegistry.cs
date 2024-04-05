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
                }
            }
            return _instance;
        }
    }

    public Resource EMPTY;
    public Resource RAW_IRON_ORE;

    public List<Resource> GetResourceList() {
        List<Resource> resourceList = new();
        
        resourceList.Add(EMPTY);
        resourceList.Add(RAW_IRON_ORE);
        
        return resourceList;
    }
}
