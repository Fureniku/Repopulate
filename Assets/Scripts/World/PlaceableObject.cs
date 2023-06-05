using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    
    private Material lastMat;
    private BuildingGrid parentGrid;
    
    [SerializeField] private Vector3Int size;
    [SerializeField] private bool mustBeGrounded = false;
    [SerializeField] private bool wallMount = false;

    void Start() {
        
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
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }

        lastMat = mat;
    }

    public BuildingGrid GetParentGrid() {
        return parentGrid;
    }

    public Vector3Int GetSize() {
        return size;
    }

    public bool MustBeGrounded() {
        return mustBeGrounded;
    }

    public bool WallMounted() {
        return wallMount;
    }
}
