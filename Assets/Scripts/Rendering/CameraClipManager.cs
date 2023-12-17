using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClipManager : MonoBehaviour {

    private readonly int celestialSmallLayerId = 9;
    private readonly int celestialLargeLayerId = 9;
    
    private readonly float distanceNormal = 1000f;
    private readonly float distanceCelestialSmall = 5000f;
    private readonly float distanceCelestialLarge = 1000000f;
    
    void Start() {
        Camera cam = GetComponent<Camera>();
        float[] distances = new float[32];
        
        for (int i = 0; i < distances.Length; i++) {
            if (i != celestialSmallLayerId && i != celestialLargeLayerId) {
                distances[i] = distanceNormal;
            }
        }
        
        distances[celestialSmallLayerId] = distanceCelestialSmall;
        distances[celestialLargeLayerId] = distanceCelestialLarge;
        
        cam.layerCullDistances = distances;
    }
}
