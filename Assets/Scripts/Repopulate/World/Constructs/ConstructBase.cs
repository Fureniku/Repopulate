using Repopulate.Utils;
using Repopulate.World.Constructs;
using UnityEngine;

public abstract class ConstructBase : MonoBehaviour {
    
    [SerializeField] protected ConstructGrid _grid;
    [SerializeField] protected GridSize _space;
    
    public ConstructGrid GetGrid() {
        return _grid;
    }

    public GridSize getOccupiedSpace() {
        return _space;
    }
}
