using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    
    private Material lastMat;
    private BuildingGrid parentGrid;
    [SerializeField] private bool showDebugSizeBox;

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

    public void SetRotation(Quaternion rotation) {
        transform.rotation = rotation;
    }

    public BuildingGrid GetParentGrid() {
        return parentGrid;
    }

    private void OnDrawGizmos() {
        if (showDebugSizeBox) {
            Matrix4x4 originalMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Vector3Int size = item.GetSize();
            float x = Mathf.Max(size.x / 2.0f, 0.5f);
            float y = Mathf.Max(size.y / 2.0f, 0.5f);
            float z = Mathf.Max(size.z / 2.0f, 0.5f);
            Gizmos.color = new Color(0.5f, 0.5f, 0.0f, 0.3f);

            Gizmos.DrawCube(Vector3.zero + new Vector3(x, y, z), item.GetSize());

            Gizmos.matrix = originalMatrix;
        }
    }
}
