using UnityEngine;

namespace Repopulate.Utils {
    public class CompoundColliderCreator : MonoBehaviour {

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
                newCollider.center = childCollider.center + childCollider.transform.position;
                newCollider.size = childCollider.size;

                // Disable child colliders
                //childCollider.enabled = false;
            }

            Debug.Log($"Added {childColliders.Length} new colliders");
        }
    }
}
