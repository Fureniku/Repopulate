using UnityEngine;

public class SolarPosition : MonoBehaviour {

    [SerializeField] private Vector3Int gridSpace;
    [SerializeField] private Vector3 solarPosition;
    
    private float gridScale;
    private Vector3 localPosition;

    void Start() {
        gridScale = GameManager.Instance.GetSolarGridScale(); //Set to 10,000 
    }

    void Update() {
        localPosition = transform.position;
        for (int i = 0; i < 3; i++) {
            if (localPosition[i] > gridScale) {
                localPosition[i] -= gridScale * 2;
                gridSpace[i]++;
            } else if (localPosition[i] < -gridScale) {
                localPosition[i] += gridScale * 2;
                gridSpace[i]--;
            }
        }
        
        transform.position = localPosition;
        solarPosition = (Vector3)gridSpace * (gridScale * 2) - localPosition*-1;
    }

    public Vector3Int GetGridSpace() => gridSpace;

    //Returns the "real" position instead of the current transform.position
    public Vector3 GetSolarPosition() => solarPosition;
    
    //Gets the vector offset between two objects on a solar scale.
    public Vector3 GetOffsetVector(Vector3 otherPos) => solarPosition - otherPos;

    //Returns true if the other grid is directly touching this grid, or is the same grid.
    public bool IsDirectNeighbour(Vector3Int otherPos) => GetGridDistance(otherPos) <= 1;

    //Get the number of grid spaces between this and the other position
    public int GetGridDistance(Vector3Int otherPos) => Mathf.Abs(gridSpace.x - otherPos.x) + Mathf.Abs(gridSpace.y - otherPos.y) + Mathf.Abs(gridSpace.z - otherPos.z);
    
    //Get the difference in value between this and the other grid space
    public Vector3Int GetGridOffset(Vector3Int otherPos) => gridSpace - otherPos;
    
    
    //TODO Currently returning in solar space; needs to be in local space.
    public Vector3 GetOpposition() {
        Vector3 cubeCenter = GetLocalSpaceFromSolar();
        Vector3 directionToOrigin = cubeCenter - Vector3.zero;
        directionToOrigin.Normalize();
        return cubeCenter - directionToOrigin * gridScale;
    }
    
    public Vector3Int GetGridOffsetClamped(Vector3Int otherPos) {
        Vector3 offset = GetGridOffset(otherPos);

        int x = (int) Mathf.Clamp(offset.x, -1, 1);
        int y = (int) Mathf.Clamp(offset.y, -1, 1);
        int z = (int) Mathf.Clamp(offset.z, -1, 1);

        return new Vector3Int(x, y, z);
    } 

    //Takes solar scale and strips off gridspace values to give the exact position within the object's local grid.
    public Vector3 GetLocalSpaceFromSolar() {
        return solarPosition - GetGridSpaceFromSolar() * (gridScale * 2);
    }

    public Vector3 GetGridSpaceFromSolar() {
        Vector3 solPos = solarPosition / (gridScale * 2);
        int x = Mathf.RoundToInt(solPos.x);
        int y = Mathf.RoundToInt(solPos.y);
        int z = Mathf.RoundToInt(solPos.z);
        return new Vector3(x, y, z);
    }

    public Vector3 GetDirectionFromPosition(Vector3 otherPos) {
        Vector3 direction = GetSolarPosition() - otherPos;
        
        return direction.normalized;
    }

    public float GetSolarDistance() {
        return GetSolarDistance(Vector3.zero, GetSolarPosition());
    }

    public float GetSolarDistance(Vector3 target) {
        return GetSolarDistance(target, GetSolarPosition());
    }

    public float GetSolarDistance(SolarPosition target) {
        return GetSolarDistance(target.GetSolarPosition(), GetSolarPosition());
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static float GetSolarDistance(Vector3 target, Vector3 self) {
        //Profiler suggests the scale system is actually slower, but I want to test in real situation when up to 8-9 digits
        /*float scaleFactor = Utilities.GetScaleFactor(target, self);
        Vector3 start = target / scaleFactor;
        Vector3 end = self / scaleFactor;
        float result = Vector3.Distance(start, end) * scaleFactor;*/
        return Vector3.Distance(target, self);
    }

    public Vector3 GetFakedPosition(Vector3 target, float distance) {
        return GetFakedPosition(GetSolarPosition(), target, distance);
    }
    
    public Vector3 GetFakedOffset(Vector3 target, float distance) {
        return GetFakedOffset(GetSolarPosition(), target, distance);
    }

    public static Vector3 GetFakedPosition(Vector3 self, Vector3 target, float distance) {
        Debug.DrawLine(self, target, Color.green);
        return self + (target - self).normalized * distance;
    }

    public static Vector3 GetFakedOffset(Vector3 self, Vector3 target, float distance) {
        Vector3 fakedPos = GetFakedPosition(self, target, distance);
        return self - fakedPos;
    }

    public static Vector3 GetLocalSpaceFromSolar(Vector3 solarPos) {
        return solarPos - GetGridSpaceFromSolar(solarPos) * (GameManager.Instance.GetSolarGridScale() * 2);
    }
    
    public static Vector3 GetGridSpaceFromSolar(Vector3 solarPos) {
        Vector3 solPos = solarPos / (GameManager.Instance.GetSolarGridScale() * 2);
        int x = Mathf.RoundToInt(solPos.x);
        int y = Mathf.RoundToInt(solPos.y);
        int z = Mathf.RoundToInt(solPos.z);
        return new Vector3(x, y, z);
    }

    public Vector3 GetSolarScaleFromGrid() {
        return (Vector3)gridSpace * gridScale;
    }
}