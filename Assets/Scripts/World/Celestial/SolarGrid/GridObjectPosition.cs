using System;
using UnityEngine;

public class GridObjectPosition : MonoBehaviour {
    
    public Vector2 gridSpace;
    [SerializeField] private float gridScale = 100f;

    // Update is called once per frame
    void Update() {
        Vector3 pos = transform.position;
        if (pos.x > gridScale) {
            pos.x -= gridScale*2;
            gridSpace.x++;
        }

        if (pos.x < -gridScale) {
            pos.x += gridScale*2;
            gridSpace.x--;
        }
        
        if (pos.z > gridScale) {
            pos.z -= gridScale*2;
            gridSpace.y++;
        }

        if (pos.z < -gridScale) {
            pos.z += gridScale*2;
            gridSpace.y--;
        }
        
        transform.position = pos;
    }
}
