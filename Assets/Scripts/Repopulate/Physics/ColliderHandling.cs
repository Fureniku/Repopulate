using Repopulate.Player;
using UnityEngine;

namespace Repopulate.Physics {
    public class ColliderHandling : MonoBehaviour {

        [SerializeField] private Collider mainCollider;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag.Equals("Player")) {
                UnityEngine.Physics.IgnoreCollision(other, mainCollider);
                other.GetComponent<DroidController>().ForceNotGroundedState(true);
            }
        }
    
        private void OnTriggerExit(Collider other) {
            if (other.gameObject.tag.Equals("Player")) {
                UnityEngine.Physics.IgnoreCollision(other, mainCollider, false);
                other.GetComponent<DroidController>().ForceNotGroundedState(false);
            }
        }
    }
}
