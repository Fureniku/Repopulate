using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMoveController : MonoBehaviour {
    
    [SerializeField] private bool controlActive;

    private Rigidbody rb;

    private void Active() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (controlActive) {
            if (Input.GetKey(KeyCode.W)) {
                rb.AddForce(transform.forward);
            }
        }
    }
}
