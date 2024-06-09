using System;
using UnityEngine;

public class PlanetManager : CelestialBody {
    
    [SerializeField] private PlanetOrbitalSystem orbitSystem;

    

    void Start() {

    }

    public void Create(PlanetOrbitalSystem system) {
        orbitSystem = system;
    }

    public PlanetOrbitalSystem GetOrbitSystem() {
        return orbitSystem;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Ship entered trigger; switch to planet orbit");
        GameManager.Instance.GetShipController().SetAvailablePlanet(this);
    }
}
