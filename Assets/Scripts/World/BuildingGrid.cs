using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour {
    
    [SerializeField] private Vector3Int gridSize;

    private bool[,,] gridSpaces; // Represents the availability of each grid space

    [SerializeField] private Vector3Int[] preOccupiedSlots;

    private void Awake() {
        gridSpaces = new bool[gridSize.x, gridSize.y, gridSize.z];

        for (int i = 0; i < preOccupiedSlots.Length; i++) {
            Vector3Int s = preOccupiedSlots[i];
            gridSpaces[s.x, s.y, s.z] = true;
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
        return transform.position + transform.parent.rotation * gridSpace;
    }

    public Quaternion GetPreviewRotation() {
        return transform.parent.rotation;
    }

    public Vector3Int IntFromVec3(Vector3 vec3) {
        return new Vector3Int((int)Math.Floor(vec3.x), (int)Math.Floor(vec3.y), (int)Math.Floor(vec3.z));
    }

    public bool CheckGridSpaceAvailability(Vector3Int startGridSpace, Vector3Int size) {
        // Check if the required grid spaces for a block are available
        for (int x = startGridSpace.x; x < startGridSpace.x + size.x; x++) {
            for (int y = startGridSpace.y; y < startGridSpace.y + size.y; y++) {
                for (int z = startGridSpace.z; z < startGridSpace.z + size.z; z++) {
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

    private void OccupyGridSpaces(Vector3Int startGridSpace, Vector3Int size) {
        // Occupy the required grid spaces for a block
        for (int x = startGridSpace.x; x < startGridSpace.x + size.x; x++) {
            for (int y = startGridSpace.y; y < startGridSpace.y + size.y; y++) {
                for (int z = startGridSpace.z; z < startGridSpace.z + size.z; z++) {
                    gridSpaces[x, y, z] = true;
                }
            }
        }
    }

    private void ReleaseGridSpaces(Vector3Int startGridSpace, Vector3Int size) {
        // Release the occupied grid spaces of a block
        for (int x = startGridSpace.x; x < startGridSpace.x + size.x; x++) {
            for (int y = startGridSpace.y; y < startGridSpace.y + size.y; y++) {
                for (int z = startGridSpace.z; z < startGridSpace.z + size.z; z++) {
                    gridSpaces[x, y, z] = false;
                }
            }
        }
    }

    public void PlaceBlock(Vector3Int gridSpace, GameObject blockPrefab) {
        // Check if the block's grid space is available
        if (CheckGridSpaceAvailability(gridSpace, Vector3Int.one)) {
            GameObject newBlock = Instantiate(blockPrefab, transform.position + transform.parent.rotation * gridSpace, transform.parent.rotation);
            newBlock.transform.SetParent(transform);
            newBlock.GetComponent<PlaceableObject>().Place(this);
            

            // Occupy the grid space
            OccupyGridSpaces(gridSpace, newBlock.GetComponent<PlaceableObject>().GetSize());
            OnGridChanged();
        }
        else {
            Debug.Log($"Invalid or occupied space {gridSpace}");
        }
    }

    public void PlaceMultiGridBlock(Vector3 position, Vector3Int size, GameObject blockPrefab) {
        Vector3 snappedPosition = GetHitSpace(position);
        Vector3Int startGridSpace = new Vector3Int((int)Mathf.Floor(snappedPosition.x), (int)Mathf.Floor(snappedPosition.y),
            (int)Mathf.Floor(snappedPosition.z));

        // Check if the required grid spaces for the block are available
        if (CheckGridSpaceAvailability(startGridSpace, size)) {
            GameObject newBlock = Instantiate(blockPrefab, snappedPosition, Quaternion.identity);
            newBlock.transform.SetParent(transform);

            // Occupy the required grid spaces
            OccupyGridSpaces(startGridSpace, size);
            OnGridChanged();
        }
    }

    public void RemoveBlock(GameObject block) {
        // Get the occupied grid spaces of the block
        Vector3Int startGridSpace = new Vector3Int(Mathf.RoundToInt(block.transform.position.x),
            Mathf.RoundToInt(block.transform.position.y), Mathf.RoundToInt(block.transform.position.z));

        // Release the occupied grid spaces
        ReleaseGridSpaces(startGridSpace, Vector3Int.one);

        Destroy(block);
        OnGridChanged();
    }

    private void OnDrawGizmos() {
        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                for (int z = 0; z < gridSize.z; z++) {
                    DrawRotatedCube(transform.position, new Vector3(x, y, z));
                }
            }
        }
    }

    private void DrawRotatedCube(Vector3 origin, Vector3 offset) {
        Vector3 start = origin + offset;
        Vector3 point1 = RotatePointAroundPivot(start, origin);
        Vector3 point2 = RotatePointAroundPivot(start + Vector3.forward, origin);
        Vector3 point3 = RotatePointAroundPivot(start + Vector3.right, origin);
        Vector3 point4 = RotatePointAroundPivot(new Vector3(start.x + 1, start.y, start.z + 1), origin);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point1, point3);
        Gizmos.DrawLine(point2, point4);
        Gizmos.DrawLine(point3, point4);

        Vector3 point5 = point1 + Vector3.up;
        Vector3 point6 = point2 + Vector3.up;
        Vector3 point7 = point3 + Vector3.up;
        Vector3 point8 = point4 + Vector3.up;
        
        Gizmos.DrawLine(point5, point6);
        Gizmos.DrawLine(point5, point7);
        Gizmos.DrawLine(point6, point8);
        Gizmos.DrawLine(point7, point8);
        
        Gizmos.DrawLine(point1, point5);
        Gizmos.DrawLine(point2, point6);
        Gizmos.DrawLine(point3, point7);
        Gizmos.DrawLine(point4, point8);
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot) {
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = transform.parent.eulerAngles.y;
        Vector3 direction = point - pivot;
        float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);

        float newX = direction.x * (cosAngle + axis.x * axis.x * (1 - cosAngle))
                     + direction.y * (axis.x * axis.y * (1 - cosAngle) - axis.z * sinAngle)
                     + direction.z * (axis.x * axis.z * (1 - cosAngle) + axis.y * sinAngle);

        float newY = direction.x * (axis.y * axis.x * (1 - cosAngle) + axis.z * sinAngle)
                     + direction.y * (cosAngle + axis.y * axis.y * (1 - cosAngle))
                     + direction.z * (axis.y * axis.z * (1 - cosAngle) - axis.x * sinAngle);

        float newZ = direction.x * (axis.z * axis.x * (1 - cosAngle) - axis.y * sinAngle)
                     + direction.y * (axis.z * axis.y * (1 - cosAngle) + axis.x * sinAngle)
                     + direction.z * (cosAngle + axis.z * axis.z * (1 - cosAngle));

        return pivot + new Vector3(newX, newY, newZ);
    }
}