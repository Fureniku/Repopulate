using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCelestialBody : MonoBehaviour {

    [SerializeField] private PlanetManager planet;
    [SerializeField] private float renderDistance = 20000;
    [SerializeField] private bool alwaysRender = false;
    [SerializeField] private Vector3 minSize = Vector3.one;
    
    private ShipMoveController ship;
    private SolarPosition shipPosition;
    private SolarPosition bodyPosition;
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
    
    void Update() {
        if (bodyPosition.GetGridSpace() == shipPosition.GetGridSpace()) {
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
            transform.position = ship.transform.position - fakeOffset;// + (Vector3) offset * solarScale;

            transform.localScale = Vector3.Lerp(minSize, maxSize, normalized);
            return;
        }

        meshRenderer.enabled = false;
    }
    
    //Caveats that need fixing:
    //- Scaling should be a curve, reducing as further away. Currently the scaling is too extreme/fast when far
    //- Planet flickers when changing gridspace as it follows the player ship. Can we fix this?
}
