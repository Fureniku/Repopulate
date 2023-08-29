using System;
using UnityEngine;

public class PlanetManager : MonoBehaviour {

    [SerializeField] public Transform scaleTargetObject; //TODO private later
    [SerializeField] private float scaleMin = 1;
    [SerializeField] private float scaleMax = 100;
    [SerializeField] private float distanceMin = 1000;
    [SerializeField] private float distanceMax = 2000;
    [SerializeField] private float scale;
    [SerializeField] private float scaleDistance;
    [SerializeField] private float scaledDistance;
    
    [SerializeField] private SolarSystemManager solarSystem;
    [SerializeField] private PlanetOrbitalSystem orbitSystem;
    

    void Awake() {
        //scaleTargetObject = GameManager.Instance.GetShipController().transform;
    }

    public void Create(PlanetOrbitalSystem system) {
        orbitSystem = system;
    }

    void Update() {
        scaleDistance = Vector3.Distance(scaleTargetObject.position, transform.position);
        scaledDistance = Mathf.Clamp01((scaleDistance - distanceMin) / (distanceMax - distanceMin));
        scale = Mathf.Lerp(scaleMax, scaleMin, scaledDistance);

        transform.localScale = new Vector3(scale, scale, scale);
    }

    public PlanetOrbitalSystem GetOrbitSystem() {
        return orbitSystem;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Ship entered trigger; switch to planet orbit");
    }
}
