using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using UnityEngine;

namespace Repopulate.World.Constructs {
    [RequireComponent(typeof(BoxCollider))]
    public class PlaceableConstruct : PlaceableBase<Construct>, IGridHolder {
        
        [Tooltip("True if this object is placed alongside the parent prefab. Will update its stats in-editor!")]
        [SerializeField] private bool prefabPlaced = false;

        [Header("Prefab Information")]
        [SerializeField] protected ConstructGrid _grid;
        [SerializeField] protected GridSize _space;

        public void OnValidate() {
            _collider = TryGetComponent(out BoxCollider col) ? col : gameObject.AddComponent<BoxCollider>();
            if (_placeable != null) {
                Vector3Int size = _placeable.Size;
                float x = Mathf.Max(size.x / 2.0f, 0.5f);
                float y = Mathf.Max(size.y / 2.0f, 0.5f);
                float z = Mathf.Max(size.z / 2.0f, 0.5f);
                _collider.center = new Vector3(x, y, z);
                _collider.size = _placeable.Size;
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
                    _space.size = _placeable.Size;
                    _grid.AttemptAddOccupiedSlot(_space);
                }
            }
        }

        public bool RequiresGrid() {
            return _placeable.RequireGrid;
        }

        public void Place(ConstructGrid newGrid, GridSize occupiedSpace) {
            _grid = newGrid;
            _space = occupiedSpace;
        }

        public void SetRotation(Quaternion rotation) {
            transform.rotation = rotation;
        }

        public ConstructGrid Grid() {
            return _grid;
        }
    }
}
