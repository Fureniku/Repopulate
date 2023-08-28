using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMoveController : MonoBehaviour {

    [SerializeField] private bool controlActive;
    [SerializeField] private Camera cam;

    [SerializeField] private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    public float maxVelocity = 5.0f; // Adjust this to your desired max speed
    public float acceleration = 1.0f;
    public float deceleration = 2.0f;

    private bool strafeModifier = false;

    private Vector2 rotateInput;
    private Vector2 strafeInput;
    private float forwardInput;

    [SerializeField] private ThrusterRingController front_ring;
    [SerializeField] private ThrusterRingController rear_ring;
    [SerializeField] private MainThrusterControl main_thruster;

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
            Vector2 burnInputFront = GetBurnVector(true);
            Vector2 burnInputRear = GetBurnVector(false);

            front_ring.ManualBurnVector(burnInputFront);
            rear_ring.ManualBurnVector(burnInputRear);

            if (forwardInput != 0) {
                main_thruster.Burn(forwardInput);
            }
        }
    }

    public void AddForce(Vector3 position, Vector3 forceStrength) {
        rb.AddForceAtPosition(forceStrength * Time.deltaTime, position);
        Vector3 endPoint = position + forceStrength.normalized * 20f;

        // Draw the debug ray
        Debug.DrawRay(position, endPoint - position, Color.red);
    }

    private Vector2 GetBurnVector(bool frontRing) {
        Vector2 burnVector = new Vector2(0, 0);
        Vector2 rotate = rotateInput * (frontRing ? 1 : -1);

        burnVector.x = Mathf.Clamp(strafeModifier ? strafeInput.x * -1 : rotate.x, -1, 1);
        burnVector.y = Mathf.Clamp((strafeModifier ? rotate.y : 0) + strafeInput.y, -1, 1);

        return burnVector;
    }

    public void HandleRotate(InputAction.CallbackContext context) => rotateInput = context.ReadValue<Vector2>();
    public void HandleStrafe(InputAction.CallbackContext context) => strafeInput = context.ReadValue<Vector2>() * -1;
    public void HandleForward(InputAction.CallbackContext context) => forwardInput = context.ReadValue<float>();
    public void HandleStrafeModifier(InputAction.CallbackContext context) => strafeModifier = context.ReadValueAsButton();

    public void SetActive(bool active) {
        controlActive = active;
        cam.gameObject.SetActive(active);
    }
}
