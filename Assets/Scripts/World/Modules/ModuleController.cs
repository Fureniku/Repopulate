using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour, UIFillable {

    [SerializeField] private bool isBuilt = false;
    [SerializeField] private bool isEnterable = false; //Whether there should be doors, oxygen, etc
    [SerializeField] private int buildTime;
    //[SerializeField] private DoorController innerDoorController;
    //[SerializeField] private DoorController stationDoorController;
    [SerializeField] private BuildingGrid grid;
    
    private float oxygenPressure;

    private int currentBuilt;
    
    void FixedUpdate() {
        if (!isBuilt) {
            if (currentBuilt < buildTime) {
                currentBuilt++;
            } else {
                isBuilt = true;
                Debug.Log("Module constructed!");
                if (isEnterable) {
                    //stationDoorController.ForceDoorState(true);
                    //innerDoorController.ForceDoorState(true);
                }
            }
        }
    }
    
    public float GetProgress() {
        return currentBuilt / (float) buildTime;
    }

    public float GetProducedOxygen() {
        List<GameObject> objects = grid.GetAllAttachedObjects();
        oxygenPressure = 0;

        for (int i = 0; i < objects.Count; i++) {
            OxygenProducer oxygen = objects[i].GetComponent<OxygenProducer>();
            if (oxygen != null) {
                oxygenPressure += oxygen.GetProducedAmount();
            }
        }
        
        return oxygenPressure;
    }
}
