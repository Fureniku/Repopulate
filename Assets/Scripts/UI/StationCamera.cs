using UnityEngine;
using UnityEngine.InputSystem;

public class StationCamera : MonoBehaviour {
    
    public Transform[] targetList;
    public Transform target;
    public float minDistance = 50.0f; // Distance from the object
    public float maxDistance = 750.0f; // Distance from the object
    public float distance = 300.0f; // Distance from the object
    public float sensitivity = 2.0f; // Mouse sensitivity

    private Vector2 rotationInput;
    private float currentXRotation = 0;
    private float zoom;
    private int id = 0;

    private void Start() {
        target = targetList[0];
        gameObject.SetActive(false);
    }

    private void Update() {
        if (GameManager.Instance.GetShipController().cursorActive) {
            rotationInput = Mouse.current.delta.ReadValue() * sensitivity;
        }

        RotateCamera();

        if (zoom != 0) {
            distance = Mathf.Clamp(distance + (zoom * Time.deltaTime * -10f), minDistance, maxDistance);
        }
    }

    public void HandleZoom(InputAction.CallbackContext context) {
        zoom = context.ReadValue<float>();
    }

    public void HandleTargetSwitch(InputAction.CallbackContext context) {
        if (context.performed) {
            if (id < targetList.Length-1) {
                id++;
            } else {
                id = 0;
            }
            target = targetList[id];
        }
    }

    private void RotateCamera() {
        // Update camera rotation based on mouse input
        currentXRotation -= rotationInput.y;
        currentXRotation = Mathf.Clamp(currentXRotation, -89f, 89f); // Limit vertical rotation

        float currentYRotation = transform.eulerAngles.y + rotationInput.x;

        Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        // Update camera position
        Vector3 newPosition = target.position - rotation * Vector3.forward * distance;
        transform.rotation = rotation;
        transform.position = newPosition;
    }

    private void OnEnable() {
        currentXRotation = transform.eulerAngles.x;
    }
}
