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
    private List<PlanetOrbitalSystem> visibleObjects = new();

    private SolarPosition ship;
    
    // Start is called before the first frame update
    void Start() {
        ship = GameManager.Instance.GetShipController().ShipPhysicsObject().GetComponent<SolarPosition>();
        for (int i = 0; i < planetCount; i++) {
            PlanetOrbitalSystem planet = Instantiate(planetPrefab, transform).GetComponent<PlanetOrbitalSystem>();
            currentPlanetDistance = Random.Range(currentPlanetDistance + minDistanceIncrease, currentPlanetDistance + maxDistanceIncrease);
            lastOrbitalSpeed = Random.Range(Mathf.Min(15f - orbitalReduction*i, lastOrbitalSpeed - orbitalReduction*i), lastOrbitalSpeed);
            planet.Setup(originPoint, 0f, currentPlanetDistance, this);
            celestialObjects.Add(planet);
        }
    }

    // Update is called once per frame
    void Update() {
        Vector2 playerPos = ship.GetGridSpace();
        for (int i = 0; i < celestialObjects.Count; i++) {
            PlanetOrbitalSystem celestialObject = celestialObjects[i];
            Vector2 celestialPos = celestialObject.GetComponent<PlanetOrbitalSystem>().GetGridSpace();
            if (visibleObjects.Contains(celestialObject)) {
                if (celestialPos != playerPos) {
                    Debug.Log("Destroy planet");
                    visibleObjects.Remove(celestialObject);
                    celestialObject.DestroyPlanet();
                }
            } else {
                if (celestialPos == playerPos) {
                    Debug.Log("Create planet");
                    celestialObject.CreatePlanet();
                    visibleObjects.Add(celestialObject);
                }
            }
        }

        for (int i = 0; i < visibleObjects.Count; i++) {
            GameObject planet = visibleObjects[i].GetPlanet().gameObject;
            planet.transform.position = visibleObjects[i].transform.localPosition * 100f;
        }
    }

    public float GetTimeScale() {
        return timeScale;
    }
}
