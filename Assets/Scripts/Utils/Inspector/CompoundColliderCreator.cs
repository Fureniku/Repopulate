using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundColliderCreator : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteColliders() {
        BoxCollider[] colliders = GetComponents<BoxCollider>();

        Debug.Log($"Deleting {colliders.Length} existing colliders");
        foreach (BoxCollider box in colliders) {
            DestroyImmediate(box);
        }
    }

    public void CombineColliders() {
        DeleteColliders();
        
        BoxCollider[] childColliders = GetComponentsInChildren<BoxCollider>();

        // Combine child colliders into a compound collider
        foreach (BoxCollider childCollider in childColliders) {
            // Copy child collider properties
            BoxCollider newCollider = gameObject.AddComponent<BoxCollider>();
            newCollider.center = childCollider.center;
            newCollider.size = childCollider.size;
            newCollider.transform.rotation = Quaternion.Euler(0, 0, 45f);

            // Disable child colliders
            //childCollider.enabled = false;
        }
        
        Debug.Log($"Added {childColliders.Length} new colliders");
    }
}
