using UnityEngine;

public class FloatingOriginManager : MonoBehaviour {
    
    [SerializeField] private Transform ship;
    [SerializeField] private Transform solarOrigin;
    [SerializeField] private float threshold = 10000.0f;
    
    private Vector3 lastPlayerPosition;
    private Vector3 offset;
    
    void Start() {
        lastPlayerPosition = ship.position;
        offset = Vector3.zero;
    }

    void Update() {
        Vector3 playerMovement = ship.position - lastPlayerPosition;

        offset += playerMovement;

        if (offset.magnitude > threshold && !IsShipOrbitingBody()) {
            Vector3 repositionOffset = -offset;

            solarOrigin.position += repositionOffset;
            ship.position += repositionOffset; // Adjust player position

            offset = Vector3.zero;
        }

        lastPlayerPosition = ship.position;
    }

    //TODO check if ships orbitting something. If so we don't want to move solar origin because the ship would move with it.
    bool IsShipOrbitingBody() {
        return false;
    }
}