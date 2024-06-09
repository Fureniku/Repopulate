using System;
using System.Collections;
using System.Collections.Generic;
using Repopulate.Player;
using UnityEngine;

public class ColliderHandling : MonoBehaviour {

    [SerializeField] private Collider mainCollider;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            Physics.IgnoreCollision(other, mainCollider);
            other.GetComponent<DroidController>().ForceNotGroundedState(true);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            Physics.IgnoreCollision(other, mainCollider, false);
            other.GetComponent<DroidController>().ForceNotGroundedState(false);
        }
    }
}
