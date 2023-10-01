using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour {

    [SerializeField][Range(0.1f, 20)] private float timeScale = 1.0f;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private int planetCount;
    [SerializeField] private Transform originPoint;

    [SerializeField] private float currentPlanetDistance = 25f;
    [SerializeField] private float minDistanceIncrease = 10f;
    [SerializeField] private float maxDistanceIncrease = 15f;
    [SerializeField] private float lastOrbitalSpeed = 25f;
    [SerializeField] private float orbitalReduction = 1f;

    private List<PlanetOrbitalSystem> celestialObjects = new();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public float GetTimeScale() {
        return timeScale;
    }
}
