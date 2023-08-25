using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHandling : MonoBehaviour {

    [SerializeField] private Collider mainCollider;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("Player");
            Physics.IgnoreCollision(other, mainCollider);
            other.GetComponent<DroidController>().ForceGroundedState(true, false);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("Player");
            Physics.IgnoreCollision(other, mainCollider, false);
            other.GetComponent<DroidController>().ForceGroundedState(false);
        }
    }
}
