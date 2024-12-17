using UnityEngine;

namespace Repopulate.World.Constructs {
    public class GridCollider : MonoBehaviour, IGridHolder {

        [SerializeField] private ConstructGrid _grid;
        
        void Awake() {
            if (_grid == null) {
                _grid = AttemptToFind();
            }
        }

        private ConstructGrid AttemptToFind() {
            ConstructGrid foundGrid = GetComponent<ConstructGrid>() 
                                      ?? transform.parent?.GetComponent<ConstructGrid>() 
                                      ?? transform.GetComponentInParent<ConstructGrid>()
                                      ?? GetComponentInChildren<ConstructGrid>();

            if (foundGrid == null) {
                Debug.LogError($"Unable to find a building grid for collider {name}");
            }

            return foundGrid;
        }

        public ConstructGrid Grid() {
            return _grid;
        }
    }
}
