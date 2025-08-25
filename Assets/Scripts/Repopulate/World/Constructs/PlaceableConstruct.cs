using System;
using Repopulate.Player;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using UnityEngine;

namespace Repopulate.World.Constructs {
    public class PlaceableConstruct : PlaceableBase<Construct>, IGridHolder, IInteractable {
        
        [Tooltip("True if this object is placed alongside the parent prefab. Will update its stats in-editor!")]
        [SerializeField] private bool prefabPlaced = false;

        [Header("Prefab Information")]
        [SerializeField][Obsolete] protected ConstructGrid _grid;
        [SerializeField] protected ClearanceTracker _clearanceTracker;
        [SerializeField] protected ConstructArea _constructArea;

        public void OnValidate() {
            _collider = TryGetComponent(out BoxCollider col) ? col : gameObject.AddComponent<BoxCollider>();
            if (_collider != null && _placeable != null) {
                Vector3Int size = _placeable.Size;
                float x = Mathf.Max(size.x / 2.0f, 0.5f);
                float y = Mathf.Max(size.y / 2.0f, 0.5f);
                float z = Mathf.Max(size.z / 2.0f, 0.5f);
                _collider.center = new Vector3(x, y, z);
                _collider.size = _placeable.Size;
            }

            if (prefabPlaced) {
                if (_constructArea == null) {
                    if (transform.parent.TryGetComponent(out ConstructArea parentArea)) {
                        _constructArea = parentArea;
                    } else {
                        Debug.LogError($"{transform.name} cannot find a construct area on parent {transform.parent.name}");
                    }
                }
                if (_grid == null) {
                    Debug.LogError("Grid was null and we're not bothering to fix that now its redundant");
                }

                if (_constructArea != null) {
                    Vector3Int approxPosition = Vector3Int.RoundToInt(transform.localPosition);
                    transform.localPosition = approxPosition;
                }
            }
        }

        public void Place(ConstructGrid newGrid, GridSize occupiedSpace) {
            _grid = newGrid;
        }

        public void SetRotation(Quaternion rotation) {
            transform.rotation = rotation;
        }

        public ConstructGrid Grid() {
            return _grid;
        }

        public ConstructArea Area() {
            return null;
        }

        public void OnInteract(PlayerControllable controllable, InteractLevel interactLevel) {
            throw new System.NotImplementedException();
        }

        public override void OnLookAt(PlayerControllable controllable) {
            Debug.Log($"Looked at {gameObject.name}");
        }

        public Construct GetConstruct() {
            return _placeable;
        }
        
        public override void OnPlaced() {
            
        }
    }
}
