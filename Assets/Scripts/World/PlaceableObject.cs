using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    
    private Material lastMat;
    private BuildingGrid parentGrid;

    [Header("Prefab Information")]
    [SerializeField] private Item item;
    [SerializeField] private GameObject rotationOrigin;
    [SerializeField] private GameObject[] visibleObjects;

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

    public void UpdateMaterials(Material mat) {
        if (mat == lastMat) {
            return;
        }

        if (visibleObjects.Length > 0) {
            for (int i = 0; i < visibleObjects.Length; i++) {
                visibleObjects[i].GetComponent<MeshRenderer>().material = mat;
            }

            return;
        }
        
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }

        lastMat = mat;
    }

    public void SetRotation(float angle) {
        Vector3 old = rotationOrigin.transform.eulerAngles;
        rotationOrigin.transform.eulerAngles = new Vector3(old.x, angle, old.z);
    }

    public BuildingGrid GetParentGrid() {
        return parentGrid;
    }
}
