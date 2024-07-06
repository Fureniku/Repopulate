using System;
using System.Collections.Generic;
using Repopulate.Utils;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Repopulate.World.Constructs {
    public class ConstructGrid : MonoBehaviour {

        [SerializeField]
        struct PrefillObjects {
        
        }
    
        [SerializeField] private Vector3Int gridSize;

        private bool[,,] gridSpaces; // Represents the availability of each grid space

        [SerializeField] public List<GridSize> occupiedSlots;

        [SerializeField] private bool showDebugGrid = true;

        private void OnValidate() {
            Awake();
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

        public void RemoveOccupiedSlot(GridSize slot) {
            if (!occupiedSlots.Remove(slot)) {
                for (int i = 0; i < occupiedSlots.Count; i++) {
                    if (occupiedSlots[i].size == slot.size && occupiedSlots[i].position == slot.position) {
                        occupiedSlots.RemoveAt(i);
                        OnValidate();
                        return;
                    }
                }
            }
            OnValidate();
        }

        public void AttemptAddOccupiedSlot(GridSize slot) {
            if (occupiedSlots.Contains(slot)) {
                return;
            }
        
            for (int i = 0; i < occupiedSlots.Count; i++) {
                if (occupiedSlots[i].size == slot.size && occupiedSlots[i].position == slot.position) {
                    return;
                }
            }
            occupiedSlots.Add(slot);
            OnValidate();
        }

        public List<GameObject> GetAllAttachedObjects() {
            List<GameObject> objects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++) {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.CompareTag("PlaceableObject")) {
                    objects.Add(child);
                }
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

        public bool CheckGridSpaceAvailability(Vector3Int gridSpace) {
            return !gridSpaces[gridSpace.x, gridSpace.y, gridSpace.z];
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
    
        private void ModifyGridSpaceOccupancy(GridSize occupancy, bool occupied) {
            Vector3Int rotatedSize = occupancy.size;
            Vector3Int rotatedStart = occupancy.position;
        
            // Release the occupied grid spaces of a block
            for (int x = rotatedStart.x; x < rotatedStart.x + rotatedSize.x; x++) {
                for (int y = rotatedStart.y; y < rotatedStart.y + rotatedSize.y; y++) {
                    for (int z = rotatedStart.z; z < rotatedStart.z + rotatedSize.z; z++) {
                        gridSpaces[x, y, z] = occupied;
                    }
                }
            }
        }

        public Vector3 GetPlacementPosition(Vector3Int gridSpace, ScriptableObjects.Construct construct) {
            Quaternion rot = transform.parent.rotation;
            Vector3 transformedPosition = rot * gridSpace;

            return transform.position + transformedPosition;
        }

        public Quaternion GetPlacementRotation(float rotation) {
            Quaternion originalRotation = transform.parent.rotation;

            Quaternion modifiedRotation = originalRotation * Quaternion.Euler(0, rotation, 0);

            return modifiedRotation;
        }

        public GridSize GetFinalPlacementOccupancy(Vector3Int size, Vector3Int startGridSpace, float rotation) {
            Vector3Int rotatedSize = GetRotatedGridSize(size, rotation);
            Vector3Int rotatedStart = GetRotatedGridPosition(startGridSpace, rotatedSize, rotation);

            return new GridSize {
                size = rotatedSize,
                position = rotatedStart
            };
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

        public void TryPlaceBlock(Vector3Int gridSpace, ScriptableObjects.Construct construct, float rotation, Direction dir) {
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

        public void PlaceBlock(Vector3Int gridSpace, ScriptableObjects.Construct construct, float rotation) {
            GameObject block = construct.Get();
            GameObject newBlock = Instantiate(block, GetPlacementPosition(gridSpace, construct), GetPlacementRotation(rotation));
            PlaceableConstruct newConstruct = newBlock.GetComponent<PlaceableConstruct>();
            GridSize occupancy = GetFinalPlacementOccupancy(newConstruct.GetPlaceable().GetSize(), gridSpace, rotation);
            newBlock.transform.SetParent(transform);
            newConstruct.Place(this, occupancy);
            
            // Occupy the grid space
            ModifyGridSpaceOccupancy(occupancy, true);
            OnGridChanged();
        }

        public void RemoveBlock(GameObject block) {
            //TODO unimplemented
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
                            Gizmos.color = Color.blue;
                            if (x < gridSize.x && y < gridSize.y && z < gridSize.z) {
                                if (gridSpaces[x, y, z]) {
                                    Gizmos.color = Color.red;
                                    Gizmos.DrawLine(pos + transformedPosition, pos + rot * new Vector3(x + 1, y + 1, z + 1));
                                    Gizmos.DrawLine(pos + transformedPosition, pos + rot * new Vector3(x + 1, y, z + 1));
                                }
                            }
                        
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
}