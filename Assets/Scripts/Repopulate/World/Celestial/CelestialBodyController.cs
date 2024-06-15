using UnityEngine;

namespace Repopulate.World.Celestial {
    public class CelestialBodyController : MonoBehaviour {

        private SolarSystemManager _solarSystem;
        private CelestialBody _celestialBody;

        private Transform _ship;

        private float _closestDistance;

        private CelestialData _data;
        private bool _initialized = false;

        public CelestialBody CelestialBody => _celestialBody;

        public void SetData(CelestialData data) {
            _data = data;
            _solarSystem = GameManager.Instance.GetSolarSystem();
            _celestialBody = GetComponentInChildren<CelestialBody>();
            _ship = GameManager.Instance.GetShipController().transform;
            _closestDistance = _data.OrbitSpeed / 2 + _data.MaximumScale;

            _celestialBody.transform.localPosition = new Vector3(data.Distance, 0, 0);
            _celestialBody.transform.localScale = Vector3.one * data.MaximumScale;
            _initialized = true;
        }

        void FixedUpdate() {
            if (!_initialized) return;
            if (_data.OrbitSpeed >= 0) {
                float rotationAmount = _data.OrbitSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, rotationAmount);
            }
        }

        void Update() {
            if (!_initialized) return;
            if (_data.CanDynamicallyRescale) {
                _celestialBody.transform.localScale = GetScale();
            }
        }

        //Adjust the scale based on distance
        protected virtual Vector3 GetScale() {
            float dist = Vector3.Distance(_ship.position, _celestialBody.transform.position);
            float scaledDistance = Mathf.Clamp01((dist - _closestDistance) / (_data.MaximumDistance - _closestDistance));
            float scale = Mathf.Lerp(_data.MaximumScale, _data.MinimumScale, scaledDistance);
            return Vector3.one * scale;
        }

        public static void create(ref GameObject gameObject) {
            CelestialBodyController cb = gameObject.AddComponent<CelestialBodyController>();
            //cb.solarSystem;
        }
    }
}

// Celestial Body Controller:
// - fixed at solar origin
// - control position in orbit (spinning on axis, can be tilted for varied spin)
// - Take in orbit speed (year) and distance, set a random rotation for start point

// Celestial Body:
// - the actual object at the orbit position
// - can have its own controller for moons etc
// - controls objects size, material, and axis spin (day)
// - ship can enter orbit by approaching large enough object