using UnityEngine;

public class FloatingOriginManager : MonoBehaviour {
    
    [SerializeField] private Transform ship; // Reference to the player spaceship
    [SerializeField] private Transform solarOrigin;   // Reference to the central point of the system
    [SerializeField] private float threshold = 10000.0f; // Distance at which the origin will be re-centered
    
    private Vector3 lastPlayerPosition;
    private Vector3 offset;
    
    void Start() {
        lastPlayerPosition = ship.position;
        offset = Vector3.zero;
    }

    void Update() {
        Vector3 playerMovement = ship.position - lastPlayerPosition;

        offset += playerMovement;

        if (offset.magnitude > threshold) {
            Vector3 repositionOffset = -offset;

            solarOrigin.position += repositionOffset;
            ship.position += repositionOffset; // Adjust player position

            offset = Vector3.zero;
        }

        lastPlayerPosition = ship.position;
    }
}