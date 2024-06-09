using System;
using Repopulate.Utils;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : BuildableBase {
    
    private Material lastMat;
    private BoxCollider _collider;
    [SerializeField] private bool showDebugSizeBox;
    [Tooltip("True if this object is placed alongside the parent prefab. Will update its stats in-editor!")]
    [SerializeField] private bool prefabPlaced = false;

    [Header("Prefab Information")]
    [SerializeField] private Construct _construct;

    public void OnValidate() {
        _collider = TryGetComponent(out BoxCollider col) ? col : gameObject.AddComponent<BoxCollider>();
        if (_construct != null) {
            Vector3Int size = _construct.GetSize();
            float x = Mathf.Max(size.x / 2.0f, 0.5f);
            float y = Mathf.Max(size.y / 2.0f, 0.5f);
            float z = Mathf.Max(size.z / 2.0f, 0.5f);
            _collider.center = new Vector3(x, y, z);
            _collider.size = _construct.GetSize();
        }

        if (prefabPlaced) {
            if (grid == null) {
                if (transform.parent.TryGetComponent(out BuildingGrid parentGrid)) {
                    grid = parentGrid;
                } else {
                    Debug.LogError($"{transform.name} cannot find a grid on parent {transform.parent.name}");
                }
            }

            if (grid != null) {
                grid.RemoveOccupiedSlot(space);
                Vector3Int approxPosition = Vector3Int.RoundToInt(transform.localPosition);
                transform.localPosition = approxPosition;
                space.position = approxPosition;
                space.size = _construct.GetSize();
                grid.AttemptAddOccupiedSlot(space);
            }
        }
    }

    public Construct GetConstruct() {
        return _construct;
    }
    
    public void Place(BuildingGrid newGrid, GridSize occupiedSpace) {
        grid = newGrid;
        space = occupiedSpace;
    }

    public void SetRotation(Quaternion rotation) {
        transform.rotation = rotation;
    }

    private void OnDrawGizmos() {
        if (showDebugSizeBox) {
            Matrix4x4 originalMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Vector3Int size = _construct.GetSize();
            float x = Mathf.Max(size.x / 2.0f, 0.5f);
            float y = Mathf.Max(size.y / 2.0f, 0.5f);
            float z = Mathf.Max(size.z / 2.0f, 0.5f);
            Gizmos.color = new Color(0.5f, 0.5f, 0.0f, 0.3f);

            Gizmos.DrawCube(new Vector3(x, y, z), _construct.GetSize());

            Gizmos.matrix = originalMatrix;
        }
    }
}
