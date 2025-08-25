using System;
using System.Collections.Generic;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using Repopulate.World.Constructs;
using UnityEngine;

public class ConstructArea : MonoBehaviour {
    
    //TODO for class:
    //construct placement
    //construct removal
    
    
    [SerializeField] private List<PlaceableConstruct> _attachedConstructs;

    //Rotation
    public void PlaceConstruct(Vector3 placePosition, Construct toPlace, float heldRotation) {
        GameObject block = toPlace.Get();
        GameObject newBlock = Instantiate(block, GetPlacementPosition(placePosition, toPlace), GetPlacementRotation(heldRotation));
        PlaceableConstruct newConstruct = newBlock.GetComponent<PlaceableConstruct>();
        _attachedConstructs.Add(newConstruct);
        newConstruct.OnPlaced();
    }
    
    
    
    
    /*public void TryPlaceBlock(Vector3Int gridSpace, Construct construct, float rotation, Direction dir) {
        // Check if the block's grid space is available
        if (CheckGridSpaceAvailability(gridSpace, Vector3Int.one, rotation)) { //TODO size
            PlaceBlock(gridSpace, construct, rotation);
        } else {
            gridSpace = GetOffsetGridSpace(gridSpace, dir);
            if (CheckGridSpaceAvailability(gridSpace, Vector3Int.one, rotation)) {
                PlaceBlock(gridSpace, construct, rotation);
            } else {
                //TODO UI feedback
                Debug.Log($"Invalid or occupied space {gridSpace} - replace me with UI feedback!");
            }
        }
    }

    public void PlaceBlock(Vector3Int gridSpace, Construct construct, float rotation) {
        GameObject block = construct.Get();
        GameObject newBlock = Instantiate(block, GetPlacementPosition(gridSpace, construct), GetPlacementRotation(rotation));
        PlaceableConstruct newConstruct = newBlock.GetComponent<PlaceableConstruct>();
        GridSize occupancy = GetFinalPlacementOccupancy(newConstruct.GetPlaceable().Size, gridSpace, rotation);
        newBlock.transform.SetParent(transform);
        newConstruct.Place(this, occupancy);
            
        // Occupy the grid space
        ModifyGridSpaceOccupancy(occupancy, true);
        OnGridChanged();
    }*/

    public Vector3 GetPlacementPosition(Vector3 placePosition, Construct construct) {
        Quaternion rot = transform.parent.rotation;
        Vector3 transformedPosition = rot * placePosition;

        return transform.position + transformedPosition;
    }

    public Quaternion GetPlacementRotation(float rotation) {
        Quaternion originalRotation = transform.parent.rotation;

        Quaternion modifiedRotation = originalRotation * Quaternion.Euler(0, rotation, 0);

        return modifiedRotation;
    }
    
    public Vector3Int GetOffsetGridSpace(Vector3Int gridSpace, Direction dir) {
        switch (dir) {
            case Direction.X_POS:
                gridSpace.x++;
                break;
            case Direction.X_NEG:
                gridSpace.x--;
                break;
            case Direction.Y_POS:
                gridSpace.y++;
                break;
            case Direction.Y_NEG:
                gridSpace.y--;
                break;
            case Direction.Z_POS:
                gridSpace.z++;
                break;
            case Direction.Z_NEG:
                gridSpace.z--;
                break;
            case Direction.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        return gridSpace;
    }
}
