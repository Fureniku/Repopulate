using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbitalSystem : MonoBehaviour {
    
    [SerializeField] private SolarSystemManager solarSystem;
    [SerializeField] private Transform orbitPoint;
    [SerializeField] private float orbitSpeed = 50.0f;  // Speed of orbit in degrees per second
    [SerializeField] private float orbitDistance = 5.0f;
    [SerializeField] private Vector2 gridSpace;

    [SerializeField] private PlanetManager planetPrefab;
    [SerializeField] private PlanetManager planet;
    public Transform scaleTargetObject;
    
    private float currentAngle = 0.0f;
    
    public void Setup(Transform orbit, float speed, float distance, SolarSystemManager system) {
        orbitPoint = orbit;
        orbitSpeed = speed;
        orbitDistance = distance;
        solarSystem = system;
    }
    
    void Update() {
        if (solarSystem != null) {
            // Calculate the new position based on the current time and orbit speed
            currentAngle += orbitSpeed * solarSystem.GetTimeScale() * Time.deltaTime;
            Vector3 newPosition = orbitPoint.position + Quaternion.Euler(0, currentAngle, 0) * new Vector3(0, 0, orbitDistance);

            // Update the position of the orbiting GameObject
            transform.position = newPosition;

            int gridX = (int) Math.Floor(transform.position.x / 10);
            int gridZ = (int) Math.Floor(transform.position.z / 10);
            
            gridSpace = new Vector2(gridX, gridZ);
        }
    }

    public void CreatePlanet() {
        planet = Instantiate(planetPrefab);
        planet.scaleTargetObject = scaleTargetObject;
    }

    public PlanetManager GetPlanet() {
        return planet;
    }

    public void DestroyPlanet() {
        Destroy(planet.gameObject);
    }

    public Vector2 GetGridSpace() {
        return gridSpace;
    }
}
