using UnityEngine;

public class GridCollider : BuildableBase {
    
    // Start is called before the first frame update
    void Awake() {
        if (grid == null) {
            grid = AttemptToFindGrid();
        }
    }

    private BuildingGrid AttemptToFindGrid() {
        BuildingGrid foundGrid = GetComponent<BuildingGrid>();

        if (foundGrid == null) {
            foundGrid = transform.parent?.GetComponent<BuildingGrid>();
        }

        if (foundGrid == null) {
            foundGrid = GetComponentInChildren<BuildingGrid>();
        }

        if (foundGrid == null) {
            Debug.LogError($"Unable to find a building grid for collider {name}");
        }

        return foundGrid;
    }
}
