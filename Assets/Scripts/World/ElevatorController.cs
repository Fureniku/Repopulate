using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ElevatorController : MonoBehaviour {

    [SerializeField][Tooltip("< 10 for down, > 10 for up")] private float strength = 10f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float maxSpeed = 5f;
    
    [SerializeField] private float damping = 0.5f;
    [SerializeField] private float maxBounceDistance = 1.0f;
    [SerializeField] private float centralisingForce = 10f;

    private CapsuleCollider cldr;

    void Awake() {
        cldr = GetComponent<CapsuleCollider>();
    }
    
    public float GetStrength() {
        return strength;
    }

    public float GetCentralisingForce() {
        return centralisingForce;
    }

    public float GetDamping() {
        return damping;
    }

    public float GetMaxBounceDistance() {
        return maxBounceDistance;
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
