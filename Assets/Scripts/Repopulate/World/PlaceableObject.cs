using System;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using Repopulate.World.Constructs;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : ConstructBase {
    
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
            if (_grid == null) {
                if (transform.parent.TryGetComponent(out ConstructGrid parentGrid)) {
                    _grid = parentGrid;
                } else {
                    Debug.LogError($"{transform.name} cannot find a grid on parent {transform.parent.name}");
                }
            }

            if (_grid != null) {
                _grid.RemoveOccupiedSlot(_space);
                Vector3Int approxPosition = Vector3Int.RoundToInt(transform.localPosition);
                transform.localPosition = approxPosition;
                _space.position = approxPosition;
                _space.size = _construct.GetSize();
                _grid.AttemptAddOccupiedSlot(_space);
            }
        }
    }

    public Construct GetConstruct() {
        return _construct;
    }
    
    public void Place(ConstructGrid newGrid, GridSize occupiedSpace) {
        _grid = newGrid;
        _space = occupiedSpace;
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
