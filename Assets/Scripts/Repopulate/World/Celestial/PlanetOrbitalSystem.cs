using System;
using UnityEngine;

namespace Repopulate.World.Celestial {
    public class PlanetOrbitalSystem : MonoBehaviour {
    
        [SerializeField] private SolarSystemManager _solarSystem;
        [SerializeField] private Transform _orbitPoint;
        [SerializeField] private float _orbitSpeed = 50.0f;  // Speed of orbit in degrees per second
        [SerializeField] private float _orbitDistance = 5.0f;
        [SerializeField] private Vector3 _orbitAngle = new Vector3(0, 1, 0);
        [SerializeField] private Vector3Int _gridSpace;

        [SerializeField] private PlanetManager _planetPrefab;
        [SerializeField] private PlanetManager _planet;
    
        private float currentAngle = 0.0f;
    
        public void Setup(Transform orbit, float speed, float distance, SolarSystemManager system) {
            _orbitPoint = orbit;
            _orbitSpeed = speed;
            _orbitDistance = distance;
            _solarSystem = system;
        }
    
        void Update() {
            if (_solarSystem != null) {
                // Calculate the new position based on the current time and orbit speed
                currentAngle += _orbitSpeed * _solarSystem.GetTimeScale() * Time.deltaTime;
                Vector3 newPosition = _orbitPoint.localPosition + Quaternion.Euler(_orbitAngle.x * currentAngle, _orbitAngle.y * currentAngle, _orbitAngle.z * currentAngle) * new Vector3(0, 0, _orbitDistance);

                // Update the position of the orbiting GameObject
                transform.localPosition = newPosition;

                int gridX = (int) Math.Floor(newPosition.x / 10);
                int gridY = (int) Math.Floor(newPosition.y / 10);
                int gridZ = (int) Math.Floor(newPosition.z / 10);
            
                _gridSpace = new Vector3Int(gridX, gridY, gridZ);
            }
        }

        public void CreatePlanet() {
            _planet = Instantiate(_planetPrefab);
        }

        public PlanetManager GetPlanet() {
            return _planet;
        }

        public void DestroyPlanet() {
            Destroy(_planet.gameObject);
        }

        public Vector3Int GetGridSpace() {
            return _gridSpace;
        }
    }
}
