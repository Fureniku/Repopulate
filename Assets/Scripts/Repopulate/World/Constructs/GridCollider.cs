using UnityEngine;

namespace Repopulate.World.Constructs {
    public class GridCollider : MonoBehaviour, IGridHolder {

        [SerializeField] private ConstructGrid _grid;
        
        // Start is called before the first frame update
        void Awake() {
            if (_grid == null) {
                _grid = AttemptToFindGrid();
            }
        }

        private ConstructGrid AttemptToFindGrid() {
            ConstructGrid foundGrid = GetComponent<ConstructGrid>() 
                                      ?? transform.parent?.GetComponent<ConstructGrid>() 
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
