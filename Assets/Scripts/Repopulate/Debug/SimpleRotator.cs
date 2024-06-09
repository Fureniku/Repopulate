using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Change this to rotate around a different axis
    [SerializeField] private float rotationSpeed = 30f; // Adjust this to change the rotation speed (degrees per second)

    private void Update() {
        // Calculate the amount of rotation for this frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply the rotation to the GameObject
        transform.Rotate(rotationAxis, rotationAmount);
    }
}
