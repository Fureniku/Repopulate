using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyController : MonoBehaviour {

    private SolarSystemManager solarSystem;
    private CelestialBody celestialBody;
    
    [Tooltip("The rotation/orbit speed in degrees per second. 1 = 5 mins for one orbit (year)")]
    [SerializeField] private float rotationSpeed = 1f; // Adjust this to change the rotation speed (degrees per second)
    
    void Start() {
        solarSystem = GameManager.Instance.GetSolarSystem();
        celestialBody = GetComponentInChildren<CelestialBody>();
    }

    void FixedUpdate() {
        if (rotationSpeed >= 0) {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }
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