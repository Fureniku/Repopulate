using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    
    private Material lastMat;
    private BuildingGrid parentGrid;

    [Header("Prefab Information")]
    [SerializeField] private Item item;
    
    public Item GetItem() {
        return item;
    }
    
    public void Place(BuildingGrid grid) {
        gameObject.layer = LayerMask.NameToLayer("BuildingGrid");
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("BuildingGrid");
        }
        parentGrid = grid;
    }

    public void SetRotation(float angle) {
        Vector3 old = transform.eulerAngles;
        transform.eulerAngles = new Vector3(old.x, angle, old.z);
    }

    public BuildingGrid GetParentGrid() {
        return parentGrid;
    }
}
