using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCelestialBody : MonoBehaviour {

    [SerializeField] private PlanetManager planet;
    [SerializeField] private float renderDistance = 100000; //How far away on solar scale this objects visible from
    [SerializeField] private bool alwaysRender = false;
    [SerializeField] private Vector3 minSize = Vector3.one;
    
    private ShipMoveController ship; //the player's ship
    private SolarPosition shipPosition; //The solar position for the player's hip
    private SolarPosition bodyPosition; //The solar position for this object
    private MeshRenderer meshRenderer;
    private float solarScale;

    private Vector3 maxSize;

    void Start() {
        ship = GameManager.Instance.GetShipController();
        shipPosition = ship.Position();
        bodyPosition = planet.GetComponent<SolarPosition>();
        meshRenderer = GetComponent<MeshRenderer>();
        solarScale = GameManager.Instance.GetSolarGridScale();
        maxSize = transform.localScale;
    }

    void UpdatePosition() {
        Vector3 offset = bodyPosition.GetOffsetVector(shipPosition.GetSolarPosition());

        // Set position based on offset
        transform.position = ship.transform.position + offset;

        // Calculate the distance from the player's ship
        float distance = offset.magnitude;

        // Determine scale based on distance
        //float scaleMultiplier = Mathf.Clamp(renderDistance / distance, 0.1f, 1f);
        //transform.localScale = minSize + (maxSize - minSize) * scaleMultiplier;

        // Set visibility based on distance
        meshRenderer.enabled = alwaysRender || distance <= renderDistance;
    }
    
    void Update() {
        meshRenderer.enabled = true;
        UpdatePosition();
        /*if (bodyPosition.GetGridSpace() == shipPosition.GetGridSpace()) {
            meshRenderer.enabled = true;
            Vector3Int gridOffset = bodyPosition.GetGridOffset(shipPosition.GetGridSpace());
            transform.position = bodyPosition.GetLocalSpaceFromSolar() + (Vector3) gridOffset * (solarScale * 2);
            return;
        }

        float distance = bodyPosition.GetSolarDistance(shipPosition);
        
        if (distance < renderDistance || alwaysRender) {
            meshRenderer.enabled = true;
            float dist = Mathf.Clamp(bodyPosition.GetSolarDistance(shipPosition), 0, renderDistance);
            float normalized = 1 - dist / (renderDistance);
            
            Vector3Int offset = bodyPosition.GetGridOffsetClamped(shipPosition.GetGridSpace());
            Vector3 fakeOffset = shipPosition.GetFakedOffset(bodyPosition.GetSolarPosition(), 10000f);
            
            //Maybe get the offset position of the position on a line from the ship's position, and use that?
            
            //TODO only very briefly tested!! test more!
            transform.position = shipPosition.GetFakedPosition(bodyPosition.GetSolarPosition(), 10000f);// + (Vector3) offset * solarScale;

            transform.localScale = Vector3.Lerp(minSize, maxSize, normalized);
            return;
        }

        meshRenderer.enabled = false;*/
    }

    private void UpdatePosition(int i) {
        int gridDistance = bodyPosition.GetGridDistance(shipPosition.GetGridSpace());

        if (gridDistance == 0) {
            Debug.Log("Same space!");
            transform.position = planet.transform.position;
        } else {
            /*Debug.Log($"Grid distance: {gridDistance}");
            transform.position = planet.transform.position + (Vector3)bodyPosition.GetGridOffsetClamped(shipPosition.GetGridSpace()) * (solarScale * 2);

            float normalized = 1 - gridDistance / 10f;
            transform.localScale = Vector3.Lerp(minSize, maxSize, normalized);*/
            transform.position = bodyPosition.GetOpposition();
        }

    }

    private void OnDrawGizmos() {
        
        if (bodyPosition != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(bodyPosition.GetSolarPosition(), 100f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(shipPosition.GetSolarPosition(), 100f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(shipPosition.GetFakedPosition(bodyPosition.GetSolarPosition(), 10000f), 100f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shipPosition.GetFakedOffset(bodyPosition.GetSolarPosition(), 10000f), 100f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(bodyPosition.GetSolarScaleFromGrid(), 100f);
        }
    }

    //Caveats that need fixing:
    //- Scaling should be a curve, reducing as further away. Currently the scaling is too extreme/fast when far
    //- Planet flickers when changing gridspace as it follows the player ship. Can we fix this?
}
