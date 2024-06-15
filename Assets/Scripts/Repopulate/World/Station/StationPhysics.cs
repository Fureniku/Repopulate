using UnityEngine;

namespace Repopulate.World.Station {
    public class StationPhysics : MonoBehaviour {

        private Rigidbody rb;

        [SerializeField] private Vector3 centreMass;
        [SerializeField] private Vector3 inertiaTensor;

        [SerializeField] private GameObject ship;

        [SerializeField] private Vector3 startingVelocity;
    
        void Start() {
            rb = GetComponent<Rigidbody>();

            rb.centerOfMass = centreMass;
            rb.inertiaTensor = inertiaTensor;

        
        }

        void Update() {
            ship.transform.position = transform.position;
            ship.transform.rotation = transform.rotation;
        
            rb.velocity = startingVelocity;
        }
    }
}
