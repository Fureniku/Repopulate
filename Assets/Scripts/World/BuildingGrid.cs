using System;
using UnityEngine;

public class BuildingGrid : MonoBehaviour {
    public Vector3Int gridSize;
    public GameObject[] blockPrefabs; // Prefabs for different block types

    private bool[,,] gridSpaces; // Represents the availability of each grid space
    private bool activated = false; // Don't try and use gridspace stuff for in editor gizmos

    private void Awake() {
        gridSpaces = new bool[gridSize.x, gridSize.y, gridSize.z];
        activated = true;
        PlaceMultiGridBlock(new Vector3(2, 2, 3), new Vector3Int(1,2,1), blockPrefabs[1]);
        BoxCollider box = GetComponent<BoxCollider>();

        box.center = gridSize / 2 + (Vector3.one / 2);
        box.size = gridSize;
        box.enabled = true;
    }

    public Vector3Int GetHitSpace(Vector3 hit) {
        Vector3 startPoint = hit - transform.position;
        Vector3Int hitSpace = IntFromVec3(startPoint);

        int clampX = Math.Clamp(hitSpace.x, 0, gridSize.x-1);
        int clampY = Math.Clamp(hitSpace.y, 0, gridSize.y-1);
        int clampZ = Math.Clamp(hitSpace.z, 0, gridSize.z-1);

        Vector3Int clamped = new Vector3Int(clampX, clampY, clampZ);
        Debug.Log($"Hit space: {IntFromVec3(clamped)}");
        
        return IntFromVec3(clamped);
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

    public void PlaceBlock(Vector3 position, GameObject blockPrefab) {
        Vector3Int gridSpace = GetHitSpace(position);

        // Check if the block's grid space is available
        if (CheckGridSpaceAvailability(gridSpace, Vector3Int.one)) {
            GameObject newBlock = Instantiate(blockPrefab, transform.position + gridSpace, Quaternion.identity);
            newBlock.transform.SetParent(transform);

            // Occupy the grid space
            OccupyGridSpaces(gridSpace, Vector3Int.one);
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
        }
    }

    public void RemoveBlock(GameObject block) {
        // Get the occupied grid spaces of the block
        Vector3Int startGridSpace = new Vector3Int(Mathf.RoundToInt(block.transform.position.x),
            Mathf.RoundToInt(block.transform.position.y), Mathf.RoundToInt(block.transform.position.z));

        // Release the occupied grid spaces
        ReleaseGridSpaces(startGridSpace, Vector3Int.one);

        Destroy(block);
    }

    private void OnDrawGizmos() {
        // Draw the grid cubes
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    Vector3 start = transform.position + new Vector3(x, y, z);
                    Gizmos.color = activated && gridSpaces[x, y, z] ? Color.red : Color.gray;
                    Gizmos.DrawWireCube(start + Vector3.one * 0.5f, Vector3.one);

                    if (activated && gridSpaces[x, y, z])
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(start + Vector3.one * 0.5f, Vector3.one * 1.1f);
                    }
                }
            }
        }
    }
}