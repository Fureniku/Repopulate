using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Profiling;
using Vector3 = UnityEngine.Vector3;

public class SolarPosition : MonoBehaviour {
    
    [SerializeField] private Vector3 gridSpace;
    [SerializeField] private Vector3 solarPosition;
    
    private float gridScale;
    private Vector3 localPosition;

    public SolarPosition(Vector3 grid, Vector3 local) {
        gridSpace = grid;
        localPosition = local;
    }

    void Start() {
        gridScale = GameManager.Instance.GetSolarGridScale();
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
        solarPosition = gridSpace * (gridScale * 2) - localPosition*-1;
    }

    public Vector3 GetGridSpace() => gridSpace;

    public Vector3 GetSolarPosition() {
        return solarPosition;
    }

    //TODO Currently returning in solar space; needs to be in local space.
    public Vector3 GetOpposition() {
        Vector3 cubeCenter = gridSpace * (gridScale * 2);
        Vector3 directionToOrigin = cubeCenter - Vector3.zero;
        directionToOrigin.Normalize();
        return cubeCenter - directionToOrigin * gridScale;
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
}