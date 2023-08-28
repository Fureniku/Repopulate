using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour {

    [SerializeField] private float scaleMin = 1;
    [SerializeField] private float scaleMax = 100;
    [SerializeField] private float distanceMin = 1000;
    [SerializeField] private float distanceMax = 2000;
    [SerializeField] private float scale;
    [SerializeField] private float distance;
    [SerializeField] private float scaledDistance;

    void Update() {
        distance = Vector3.Distance(GameManager.Instance.GetShipController().transform.position, transform.position);
        scaledDistance = Mathf.Clamp01((distance - distanceMin) / (distanceMax - distanceMin));
        scale = Mathf.Lerp(scaleMax, scaleMin, scaledDistance);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
