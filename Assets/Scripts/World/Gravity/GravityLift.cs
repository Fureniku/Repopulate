using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class GravityLift : MonoBehaviour {

    [SerializeField][Tooltip("< 10 for down, > 10 for up")] private float strength = 10f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float radius = 5f;
    
    [SerializeField] private float damping = 0.5f;
    [SerializeField] private float maxBounceDistance = 1.0f;
    [SerializeField] private float centralisingForce = 10f;

    private CapsuleCollider cldr;

    void Awake() {
        cldr = GetComponent<CapsuleCollider>();
        cldr.isTrigger = true;
    }
    
    public float GetStrength() {
        return strength;
    }

    public void HandleForces(Rigidbody rb) {
        Vector3 elevPos = transform.position;
        Vector3 rbPos = rb.transform.position;
        
        Vector3 directionToCenter = (elevPos - rbPos).normalized;
        Vector3 localDirection = transform.InverseTransformDirection(directionToCenter);
        float distanceToCenter = Vector3.Distance(rbPos, elevPos);
        
        // Calculate the attraction force towards the center (only on X and Z axes)
        Vector3 localAttractionForce = new Vector3(localDirection.x, 0.0f, localDirection.z) * centralisingForce;
        if (localAttractionForce != Vector3.zero) {
            Debug.Log($"succ: {localAttractionForce}");
        }
        rb.AddRelativeForce(localAttractionForce);

        // Calculate the bounce force and apply damping only on X and Z axes
        Vector3 relativeVelocity = transform.InverseTransformDirection(rb.velocity) - localDirection * Vector3.Dot(transform.InverseTransformDirection(rb.velocity), localDirection);
        float bounceFactor = Mathf.Clamp01(1.0f - distanceToCenter / maxBounceDistance);
        Vector3 localBounceForce = new Vector3(-relativeVelocity.x, 0.0f, -relativeVelocity.z) * bounceFactor * damping;
        rb.AddRelativeForce(localBounceForce);
    }

    private void OnValidate() {
        cldr = GetComponent<CapsuleCollider>();
        cldr.center = new Vector3(0, height / 2f, 0);
        cldr.height = height;
        cldr.radius = radius;
    }

    private void OnDrawGizmos() {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Color col = Color.green;
        col.a = 0.25f;
        Gizmos.color = col;
        Gizmos.DrawCube(new Vector3(0, height / 2f, 0), new Vector3(radius*2, height, radius*2));
        
        Gizmos.matrix = originalMatrix;
    }
}
