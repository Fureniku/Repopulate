using UnityEngine;

public abstract class BuildableBase : MonoBehaviour {
    
    [SerializeField] protected BuildingGrid grid;
    
    public BuildingGrid GetGrid() {
        return grid;
    }
}
