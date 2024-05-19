using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour {
    [SerializeField] private bool _shouldFollowMouse = true;

    // Update is called once per frame
    void Update() {
        if (_shouldFollowMouse) {
            transform.position = Mouse.current.position.value;
        }
    }
}
