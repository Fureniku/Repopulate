using UnityEngine;

namespace Repopulate.World.Constructs {
    public class GridCollider : MonoBehaviour, IGridHolder {

        [SerializeField] private ConstructArea _grid;
        
        void Awake() {
            if (_grid == null) {
                _grid = AttemptToFind();
            }
        }

        private ConstructArea AttemptToFind() {
            ConstructArea foundGrid = GetComponent<ConstructArea>() 
                                      ?? transform.parent?.GetComponent<ConstructArea>() 
                                      ?? transform.GetComponentInParent<ConstructArea>()
                                      ?? GetComponentInChildren<ConstructArea>();

            if (foundGrid == null) {
                Debug.LogError($"Unable to find a building grid for collider {name}");
            }

            return foundGrid;
        }

        public ConstructArea Area() {
            return _grid;
        }
        
        public ConstructGrid Grid() {
            return null;
        }
    }
}
