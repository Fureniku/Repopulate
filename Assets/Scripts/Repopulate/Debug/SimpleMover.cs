using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour {

    [SerializeField] private Vector3 moveAmount;
    
    // Update is called once per frame
    void Update() {
        transform.Translate(moveAmount);
    }
}
