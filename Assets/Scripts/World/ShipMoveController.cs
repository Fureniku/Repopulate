using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMoveController : MonoBehaviour {
    
    [SerializeField] private bool controlActive;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    public float maxVelocity = 5.0f;  // Adjust this to your desired max speed
    public float acceleration = 1.0f; 
    public float deceleration = 2.0f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (controlActive) {

            /*float verticalInput = forwardInput.ReadValue<float>();
            float horizontalInput = yawInput.ReadValue<float>();

            // Calculate movement and rotation
            Vector3 moveDirection = transform.forward * verticalInput * 2f * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(0f, horizontalInput * 2f * Time.deltaTime, 0f);

            // Apply movement and rotation
            rb.MovePosition(rb.position + moveDirection);
            rb.MoveRotation(rb.rotation * rotation);*/
        }
    }

    public void AddForce(Vector3 position, Vector3 forceStrength) {
        rb.AddForceAtPosition(forceStrength * Time.deltaTime, position);
    }

    public void HandleMovement(InputAction.CallbackContext context) {
        
    }

    public void HandleRotation(InputAction.CallbackContext context) {
        Quaternion rotation = Quaternion.Euler(context.ReadValue<Vector3>() * 2f * Time.deltaTime);
        rb.MoveRotation(rb.rotation * rotation);
    }

    public void SetActive(bool active) {
        controlActive = active;
    }
}
