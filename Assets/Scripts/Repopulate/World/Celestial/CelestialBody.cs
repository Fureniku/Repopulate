using UnityEngine;

namespace Repopulate.World.Celestial {
    public class CelestialBody : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            StationPhysics station = other.GetComponent<StationPhysics>();

            if (station != null) {
                Debug.Log("Station!");
            }
        }
    }
}
