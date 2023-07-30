using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    [SerializeField][Tooltip("< 10 for down, > 10 for up")] private float strength = 10f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float maxSpeed = 5f;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * strength, ForceMode.Force);
            
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed), rb.velocity.z);
        }
    }
}
