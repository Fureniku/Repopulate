using System;
using UnityEngine;

public class GridObjectPosition : MonoBehaviour {
    
    [SerializeField] private Vector3 gridSpace;
    [SerializeField] private Vector3 solarPosition;
    private float gridScale;

    void Start() {
        gridScale = GameManager.Instance.GetSolarGridScale();
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = transform.position;
        if (pos.x > gridScale) {
            pos.x -= gridScale * 2;
            gridSpace.x++;
        }

        if (pos.x < -gridScale) {
            pos.x += gridScale * 2;
            gridSpace.x--;
        }
        
        if (pos.y > gridScale) {
            pos.y -= gridScale * 2;
            gridSpace.y++;
        }

        if (pos.y < -gridScale) {
            pos.y += gridScale * 2;
            gridSpace.y--;
        }

        if (pos.z > gridScale) {
            pos.z -= gridScale * 2;
            gridSpace.z++;
        }

        if (pos.z < -gridScale) {
            pos.z += gridScale * 2;
            gridSpace.z--;
        }
        
        transform.position = pos;
        solarPosition = gridSpace * (gridScale * 2) - pos*-1;
    }
    //Real was -9000, grid was 0. Move 2000. Real now +9000, grid now -1. Solar should be -11000
    //Real becomes +8000. Solar becaomes -12000.

    public Vector3 GetGridSpace() => gridSpace;

    public Vector3 GetSolarPosition() {
        return solarPosition;
    }
}
