using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Repopulate.World;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour {

    [SerializeField][Range(0.1f, 20)] private float timeScale = 1.0f;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private int planetCount;
    [SerializeField] private Transform originPoint;

    [Tooltip("How many real minutes should one Earth-type year take")]
    [SerializeField] private float _yearScale = 1460;

    [SerializeField] private float currentPlanetDistance = 25f;
    [SerializeField] private float minDistanceIncrease = 10f;
    [SerializeField] private float maxDistanceIncrease = 15f;
    [SerializeField] private float lastOrbitalSpeed = 25f;
    [SerializeField] private float orbitalReduction = 1f;

    [Header("Prefabs")]
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _planet;
    [SerializeField] private GameObject _moon;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private GameObject _comet;
    [SerializeField] private GameObject _dwarfPlanet;
    
    [Header("Celestial bodies")]
    [SerializeField] private List<CelestialData> _celestialData = new();

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < _celestialData.Count; i++) {
            GameObject go = null;
            switch (_celestialData[i].BodyType) {
                case CelestialType.STAR:
                    go = Instantiate(_star, transform);
                    break;
                case CelestialType.PLANET_ROCK:
                    go = Instantiate(_planet, transform);
                    break;
                case CelestialType.PLANET_GAS:
                    go = Instantiate(_planet, transform);
                    break;
                case CelestialType.ASTEROID:
                    go = Instantiate(_asteroid, transform);
                    break;
                case CelestialType.COMET:
                    go = Instantiate(_comet, transform);
                    break;
                case CelestialType.PLANET_DWARF:
                    go = Instantiate(_dwarfPlanet, transform);
                    break;
            }

            if (go != null) {
                go.name = _celestialData[i].Name + " Controller";
                CelestialBodyController cbc = go.GetComponent<CelestialBodyController>();
                _celestialData[i].SetData(cbc);
                cbc.CelestialBody.name = _celestialData[i].Name;
            }
        }
    }

    private void OnValidate() {
        float seconds = _yearScale * 60;
        float dps = 360 / seconds;
        for (int i = 0; i < _celestialData.Count; i++) {
            _celestialData[i].SetCalculatedSpeed(dps);
        }
    }

    public List<CelestialData> GetCelestialBodies() {
        return _celestialData;
    }

    public float GetTimeScale() {
        return timeScale;
    }
}
