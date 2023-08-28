using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMoveController : MonoBehaviour {
    
    [SerializeField] private bool controlActive;
    [SerializeField] private Camera cam;

    [SerializeField] private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    public float maxVelocity = 5.0f;  // Adjust this to your desired max speed
    public float acceleration = 1.0f; 
    public float deceleration = 2.0f;
    
    private float strafeInput = 0f;
    private float raiseLowerInput = 0f;

    private float pitchInput = 0f;
    private float yawInput = 0f;
    private float rollInput = 0f;

    [SerializeField] private ThrusterRingController front_ring;
    [SerializeField] private ThrusterRingController rear_ring;

    private void Awake() {
        rb.centerOfMass = (front_ring.transform.position + rear_ring.transform.position) / 2;

        /*front_ring.ScheduleBurn(EnumMoveDirection.NONE, 2);
        rear_ring.ScheduleBurn(EnumMoveDirection.NONE, 2);
        front_ring.ScheduleBurn(EnumMoveDirection.ROTATE_ROLL_NEG, 5, 1);
        rear_ring.ScheduleBurn(EnumMoveDirection.ROTATE_ROLL_NEG, 5, 1);
        front_ring.ScheduleBurn(EnumMoveDirection.ROTATE_ROLL_POS, 5, 1);
        rear_ring.ScheduleBurn(EnumMoveDirection.ROTATE_ROLL_POS, 5, 1);*/
    }

    private void FixedUpdate() {
        if (controlActive) {
            if (strafeInput > 0) {
                Debug.Log("Strafing right");
                front_ring.ManualBurn(EnumMoveDirection.STRAFE_RIGHT, strafeInput);
                rear_ring.ManualBurn(EnumMoveDirection.STRAFE_RIGHT, strafeInput);
            } else if (strafeInput < 0) {
                Debug.Log("Strafing left");
                front_ring.ManualBurn(EnumMoveDirection.STRAFE_LEFT, strafeInput);
                rear_ring.ManualBurn(EnumMoveDirection.STRAFE_LEFT, strafeInput);
            }

            if (raiseLowerInput > 0) {
                Debug.Log("Raise!");
                front_ring.ManualBurn(EnumMoveDirection.RAISE, raiseLowerInput);
                rear_ring.ManualBurn(EnumMoveDirection.RAISE, raiseLowerInput);
            } else if (raiseLowerInput < 0) {
                Debug.Log("Lower!");
                front_ring.ManualBurn(EnumMoveDirection.LOWER, raiseLowerInput);
                rear_ring.ManualBurn(EnumMoveDirection.LOWER, raiseLowerInput);
            }
            
            if (pitchInput > 0) {
                Debug.Log("Raise!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_PITCH_POS, pitchInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_PITCH_POS, pitchInput);
            } else if (pitchInput < 0) {
                Debug.Log("Lower!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_PITCH_NEG, pitchInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_PITCH_NEG, pitchInput);
            }
            
            if (yawInput > 0) {
                Debug.Log("Raise!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_YAW_POS, yawInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_YAW_POS, yawInput);
            } else if (yawInput < 0) {
                Debug.Log("Lower!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_YAW_NEG, yawInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_YAW_NEG, yawInput);
            }
            
            if (rollInput > 0) {
                Debug.Log("Raise!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_ROLL_POS, rollInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_ROLL_POS, rollInput);
            } else if (rollInput < 0) {
                Debug.Log("Lower!");
                front_ring.ManualBurn(EnumMoveDirection.ROTATE_ROLL_NEG, rollInput);
                rear_ring.ManualBurn(EnumMoveDirection.ROTATE_ROLL_NEG, rollInput);
            }
        }
    }

    public void AddForce(Vector3 position, Vector3 forceStrength) {
        //Debug.Log($"Adding force {forceStrength} at {position}");
        rb.AddForceAtPosition(forceStrength * Time.deltaTime, position);
        
        Vector3 endPoint = position + forceStrength.normalized * 20f;

        // Draw the debug ray
        Debug.DrawRay(position, endPoint - position, Color.red);
    }

    public void HandleStrafeMovement(InputAction.CallbackContext context) => strafeInput = context.ReadValue<float>();
    public void HandleRaiseLowerMovement(InputAction.CallbackContext context) => raiseLowerInput = context.ReadValue<float>();
    
    public void HandlePitchRotation(InputAction.CallbackContext context) => pitchInput = context.ReadValue<float>();
    public void HandleYawRotation(InputAction.CallbackContext context) => yawInput = context.ReadValue<float>();
    public void HandleRollRotation(InputAction.CallbackContext context) => rollInput = context.ReadValue<float>();

    public void HandleRotation(InputAction.CallbackContext context) {
        Quaternion rotation = Quaternion.Euler(context.ReadValue<Vector3>() * 2f * Time.deltaTime);
        //rb.MoveRotation(rb.rotation * rotation);
    }

    public void SetActive(bool active) {
        controlActive = active;
        cam.gameObject.SetActive(active);
    }
}
