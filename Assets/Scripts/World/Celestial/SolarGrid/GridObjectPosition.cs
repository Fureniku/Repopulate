using System;
using UnityEngine;

public class GridObjectPosition : MonoBehaviour {
    
    [SerializeField] private Vector2 gridSpace;
    private float gridScale;

    void Start() {
        gridScale = GameManager.Instance.GetSolarGridScale();
    }

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

    public Vector2 GetGridSpace() => gridSpace;
}
