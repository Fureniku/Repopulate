using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour {
    
    [SerializeField] private Vector3Int gridSize;

    private bool[,,] gridSpaces; // Represents the availability of each grid space

    [SerializeField] public GridSize[] occupiedSlots;

    [SerializeField] private bool showDebugGrid = true;

    private void OnValidate() {
        gridSpaces = new bool[gridSize.x, gridSize.y, gridSize.z];
        FillPreoccupiedSlots();
    }

    private void Awake() {
        gridSpaces = new bool[gridSize.x, gridSize.y, gridSize.z];
        FillPreoccupiedSlots();
    }

    private void FillPreoccupiedSlots() {
        foreach (GridSize s in occupiedSlots) {
            Vector3Int startPos = s.position;
            Vector3Int size = s.size;
            
            for (int x = startPos.x; x < startPos.x + size.x; x++) {
                for (int y = startPos.y; y < startPos.y + size.y; y++) {
                    for (int z = startPos.z; z < startPos.z + size.z; z++) {
                        gridSpaces[x, y, z] = true;
                    }
                }
            }
        }
    }

    public List<GameObject> GetAllAttachedObjects() {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("PlaceableObject")) {
                objects.Add(child);
            }
        }

        for (int i = 0; i < objects.Count; i++) {
            Debug.Log("Checking attached object list. Found entry: " + objects[i].name);
        }

        return objects;
    }

    private void OnGridChanged() { //TODO add event call later
        GetAllAttachedObjects();
    }

    public Vector3Int GetHitSpace(Vector3 hit) {
        Vector3 hitPositionLocal = transform.InverseTransformPoint(hit);
        Vector3Int hitSpace = IntFromVec3(hitPositionLocal);

        int clampX = Math.Clamp(hitSpace.x, 0, gridSize.x-1);
        int clampY = Math.Clamp(hitSpace.y, 0, gridSize.y-1);
        int clampZ = Math.Clamp(hitSpace.z, 0, gridSize.z-1);
        
        Vector3Int clamped = new Vector3Int(clampX, clampY, clampZ);
        
        return clamped;
    }

    public Vector3 GetPreviewPosition(Vector3Int gridSpace) {
        Debug.Log("Preview pos: " + transform.position + transform.parent.rotation * gridSpace);
        return transform.position + transform.parent.rotation * gridSpace;
    }

    [SerializeField] private bool debugLine = false;

    public Quaternion GetPreviewRotation() {
        if (debugLine) {
            Debug.Log($"Rotation: {transform.parent.rotation}");   
        }
        
        return transform.parent.rotation;
    }

    public Vector3Int IntFromVec3(Vector3 vec3) {
        return new Vector3Int((int)Math.Floor(vec3.x), (int)Math.Floor(vec3.y), (int)Math.Floor(vec3.z));
    }

    public bool CheckGridSpaceAvailability(Vector3Int startGridSpace, Vector3Int size, float rotation) {
        Vector3Int rotatedSize = GetRotatedGridSize(size, rotation);
        Vector3Int rotatedStart = GetRotatedGridPosition(startGridSpace, rotatedSize, rotation);
        
        for (int x = rotatedStart.x; x < rotatedStart.x + rotatedSize.x; x++) {
            for (int y = rotatedStart.y; y < rotatedStart.y + rotatedSize.y; y++) {
                for (int z = rotatedStart.z; z < rotatedStart.z + rotatedSize.z; z++) {
                    // Check if the grid space is out of bounds or already occupied
                    if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y || z < 0 || z >= gridSize.z ||
                        gridSpaces[x, y, z]) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private Vector3Int GetRotatedGridSize(Vector3Int size,  float rotation) {
        int sizeX = size.x;
        int sizeZ = size.z;

        if (Mathf.Approximately(rotation, 90)) {
            sizeX = size.z;
            sizeZ = size.x;
        }
        
        if (Mathf.Approximately(rotation, 270)) {
            sizeX = size.z;
            sizeZ = size.x;
        }

        return new Vector3Int(sizeX, size.y, sizeZ);
    }

    private Vector3Int GetRotatedGridPosition(Vector3Int startGridSpace, Vector3Int rotatedSize, float rotation) {
        int startX = startGridSpace.x;
        int startZ = startGridSpace.z;

        if (Mathf.Approximately(rotation, 90)) {
            startZ -= rotatedSize.z - 1;
        }
        
        if (Mathf.Approximately(rotation, 180)) {
            startX -= rotatedSize.x - 1;
        }

        return new Vector3Int(startX, startGridSpace.y, startZ);
    }
    
    private void ModifyGridSpaceOccupancy(Vector3Int startGridSpace, Vector3Int size, float rotation, bool occupied) {
        Vector3Int rotatedSize = GetRotatedGridSize(size, rotation);
        Vector3Int rotatedStart = GetRotatedGridPosition(startGridSpace, rotatedSize, rotation);
        
        // Release the occupied grid spaces of a block
        for (int x = rotatedStart.x; x < rotatedStart.x + rotatedSize.x; x++) {
            for (int y = rotatedStart.y; y < rotatedStart.y + rotatedSize.y; y++) {
                for (int z = rotatedStart.z; z < rotatedStart.z + rotatedSize.z; z++) {
                    gridSpaces[x, y, z] = occupied;
                }
            }
        }
    }

    public Vector3 GetPlacementPosition(Vector3Int gridSpace, Item item) {
        Quaternion rot = transform.parent.rotation;
        Vector3 transformedPosition = rot * gridSpace;

        return transform.position + transformedPosition; // + new Vector3(0.5f, 0, 0.5f);
    }

    public Quaternion GetPlacementRotation(Vector3Int gridspace, float rotation) {
        return Quaternion.Euler(transform.parent.rotation.eulerAngles + new Vector3(0, rotation, 0));
    }

    public void PlaceBlock(Vector3Int gridSpace, Item item, float rotation) {
        // Check if the block's grid space is available
        if (CheckGridSpaceAvailability(gridSpace, Vector3Int.one, rotation)) {
            GameObject block = item.Get();
            
            GameObject newBlock = Instantiate(block, GetPlacementPosition(gridSpace, item), GetPlacementRotation(gridSpace, rotation));
            newBlock.transform.SetParent(transform);
            newBlock.GetComponent<PlaceableObject>().Place(this);
            
            // Occupy the grid space
            ModifyGridSpaceOccupancy(gridSpace, newBlock.GetComponent<PlaceableObject>().GetItem().GetSize(), rotation, true);
            OnGridChanged();
        } else {
            //TODO UI feedback
            Debug.Log($"Invalid or occupied space {gridSpace} - replace me with UI feedback!");
        }
    }

    public void RemoveBlock(GameObject block) {
        // Get the occupied grid spaces of the block
        Vector3Int startGridSpace = new Vector3Int(Mathf.RoundToInt(block.transform.position.x),
            Mathf.RoundToInt(block.transform.position.y), Mathf.RoundToInt(block.transform.position.z));

        // Release the occupied grid spaces
        //TODO get size and rotation from targeted block
        //ModifyGridSpaceOccupancy(startGridSpace, Vector3Int.one, rotation, false);

        Destroy(block);
        OnGridChanged();
    }

    private void OnDrawGizmos() {
        if (showDebugGrid) {
            
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            for (int x = 0; x <= gridSize.x; x++) {
                for (int y = 0; y <= gridSize.y; y++) {
                    for (int z = 0; z <= gridSize.z; z++) {
                        // Calculate the position of the cell in the grid, accounting for rotation
                        Vector3 cellPosition = new Vector3(x, y, z);
                        Vector3 transformedPosition = rot * cellPosition;

                        if (x < gridSize.x && y < gridSize.y && z < gridSize.z) {
                            Gizmos.color = gridSpaces[x,y,z] ? Color.red : Color.blue;
                        } else {
                            Gizmos.color = Color.white;
                        }
                        
                        
                        
                        // Draw lines to visualize the grid
                        if (x < gridSize.x) {
                            Gizmos.DrawLine(pos + transformedPosition, pos + rot * new Vector3((x + 1), y, z));
                        }

                        if (y < gridSize.y) {
                            Gizmos.DrawLine(pos + transformedPosition, pos + rot * new Vector3(x, (y + 1), z));
                        }

                        if (z < gridSize.z) {
                            Gizmos.DrawLine(pos + transformedPosition, pos + rot * new Vector3(x, y, (z + 1)));
                        }
                    }
                }
            }
        }
    }
}