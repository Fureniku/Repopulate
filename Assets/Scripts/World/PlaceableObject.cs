using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : BuildableBase {
    
    private Material lastMat;
    private BoxCollider _collider;
    [SerializeField] private bool showDebugSizeBox;

    [Header("Prefab Information")]
    [SerializeField] private Item item;

    public void OnValidate() {
        _collider = TryGetComponent(out BoxCollider col) ? col : gameObject.AddComponent<BoxCollider>();
        if (item != null) {
            Vector3Int size = item.GetSize();
            float x = Mathf.Max(size.x / 2.0f, 0.5f);
            float y = Mathf.Max(size.y / 2.0f, 0.5f);
            float z = Mathf.Max(size.z / 2.0f, 0.5f);
            _collider.center = new Vector3(x, y, z);
            _collider.size = item.GetSize();
        }
    }

    public Item GetItem() {
        return item;
    }
    
    public void Place(BuildingGrid newGrid) {
        grid = newGrid;
    }

    public void SetRotation(Quaternion rotation) {
        transform.rotation = rotation;
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

            Gizmos.DrawCube(new Vector3(x, y, z), item.GetSize());

            Gizmos.matrix = originalMatrix;
        }
    }
}
