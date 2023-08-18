using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {
    
    protected abstract void OnInteract();
    
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            OnInteract();
        }
    }
}
