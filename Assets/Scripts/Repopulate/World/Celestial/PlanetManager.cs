using UnityEngine;

namespace Repopulate.World.Celestial {
    public class PlanetManager : CelestialBody {
    
        [SerializeField] private PlanetOrbitalSystem _orbitSystem;

        public void Create(PlanetOrbitalSystem system) {
            _orbitSystem = system;
        }

        public PlanetOrbitalSystem GetOrbitSystem() {
            return _orbitSystem;
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Ship entered trigger; switch to planet orbit");
            GameManager.Instance.GetShipController().SetAvailablePlanet(this);
        }
    }
}
