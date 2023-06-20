using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour, UIFillable {

    [SerializeField] private bool isBuilt = false;
    [SerializeField] private int buildTime;
    [SerializeField] private DoorController innerDoorController;
    [SerializeField] private DoorController stationDoorController;

    private int currentBuilt;
    
    void FixedUpdate() {
        if (!isBuilt) {
            if (currentBuilt < buildTime) {
                currentBuilt++;
            } else {
                isBuilt = true;
                Debug.Log("Module constructed!");
                stationDoorController.ForceDoorState(true);
                innerDoorController.ForceDoorState(true);
            }
        }
    }

    public void RegisterStationDoor(DoorController station) {
        stationDoorController = station;
    }
    
    public float GetProgress() {
        return currentBuilt / (float) buildTime;
    }
}
