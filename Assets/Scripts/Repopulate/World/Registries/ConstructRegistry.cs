using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConstructRegistry : ScriptableObject {
    
    private static ConstructRegistry _instance;
    
    public static ConstructRegistry Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<ConstructRegistry>("Construct_Registry");
                if (_instance == null) {
                    Debug.LogError("No instance of Construct_Registry found in Resources folder.");
                }
                
                _instance.ConstructList = _instance.GetConstructList();
                _instance.ConstructCount = _instance.ConstructList.Count;
            }
            return _instance;
        }
    }
    
    public List<Construct> ConstructList { get; private set; }
    public int ConstructCount { get; private set; }

    public Construct EMPTY;
    public Construct ALGAE_FARM_1;
    public Construct PILLAR_1;
    public Construct BATTERY_1;
    public Construct CUBE_PANELS;
    public Construct CUBE_METAL;
    public Construct WALL_LIGHT_1;

    private List<Construct> GetConstructList() {
        List<Construct> _constructList = new();

        //These MUST be in ID order.
        _constructList.Add(EMPTY);
        _constructList.Add(ALGAE_FARM_1);
        _constructList.Add(PILLAR_1);
        _constructList.Add(BATTERY_1);
        _constructList.Add(CUBE_PANELS);
        _constructList.Add(CUBE_METAL);
        _constructList.Add(WALL_LIGHT_1);
        
        return _constructList;
    }
}
