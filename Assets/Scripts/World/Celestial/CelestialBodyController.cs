using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyController : MonoBehaviour {

    private SolarSystemManager _solarSystem;
    private CelestialBody _celestialBody;

    private Transform _ship;

    private float _closestDistance;
    
    [Tooltip("The rotation/orbit speed in degrees per second. 1 = 5 mins for one orbit (year)")]
    [SerializeField] private float _orbitSpeed = 1f; // Adjust this to change the rotation speed (degrees per second)
    
    [Header("Dynamic Rescaling")]
    [SerializeField] private bool _canDynamiclyRescale = true;
    [Tooltip("The distance where scale will be at maximum, and no longer increase")]
    [SerializeField] private float _minDistance = 5000f;
    [Tooltip("The distance at which scale for the object would be minimum.")]
    [SerializeField] private float _maxDistance = 100000f;
    [Tooltip("The distance from the *planets surface* (not centre) where scale should be max")]
    [SerializeField] private float _scaleMax;
    [SerializeField] private float _scaleMin;
    
    void Start() {
        _solarSystem = GameManager.Instance.GetSolarSystem();
        _celestialBody = GetComponentInChildren<CelestialBody>();
        _ship = GameManager.Instance.GetShipController().transform;
        _closestDistance = _minDistance/2 + _scaleMax;
    }

    void FixedUpdate() {
        if (_orbitSpeed >= 0) {
            float rotationAmount = _orbitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }
    }

    void Update() {
        if (_canDynamiclyRescale) {
            _celestialBody.transform.localScale = GetScale();
        }
    }

    //Adjust the scale based on distance
    protected virtual Vector3 GetScale() {
        float dist = Vector3.Distance(_ship.position, _celestialBody.transform.position);
        float scaledDistance = Mathf.Clamp01((dist - _closestDistance) / (_maxDistance - _closestDistance));
        float scale = Mathf.Lerp(_scaleMax, _scaleMin, scaledDistance);
        return Vector3.one * scale;
    }

    public static void create(ref GameObject gameObject) {
        CelestialBodyController cb = gameObject.AddComponent<CelestialBodyController>();
        //cb.solarSystem;
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