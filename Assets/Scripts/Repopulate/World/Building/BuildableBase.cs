using Repopulate.Utils;
using UnityEngine;

public abstract class BuildableBase : MonoBehaviour {
    
    [SerializeField] protected BuildingGrid grid;
    [SerializeField] protected GridSize space;
    
    public BuildingGrid GetGrid() {
        return grid;
    }

    public GridSize getOccupiedSpace() {
        return space;
    }
}
