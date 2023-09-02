using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIHandler_Ship : MonoBehaviour {

    [SerializeField] private ShipMoveController ship;
    [SerializeField] private GameObject debugUIElement;

    private List<TMP_Text> texts = new();

    private Rigidbody shipRigidbody;
    private GridObjectPosition gridPos;
    
    private readonly int uiElementCount = 5;

    void Start() {
        shipRigidbody = ship.ShipPhysicsObject().GetComponent<Rigidbody>();
        gridPos = ship.ShipPhysicsObject().GetComponent<GridObjectPosition>();
        for (int i = 0; i < uiElementCount; i++) {
            texts.Add(Instantiate(debugUIElement, transform).GetComponent<TMP_Text>());
        }
        
    }

    // Update is called once per frame
    void Update() {
        texts[0].SetText($"Speed: {shipRigidbody.velocity.magnitude}");
        texts[1].SetText($"Angular Velocity: {shipRigidbody.angularVelocity}");
        texts[2].SetText($"Grid Position: {gridPos.GetGridSpace()}");
        texts[3].SetText($"Solar Position: {gridPos.GetSolarPosition()}");
        texts[4].SetText($"Position: {ship.transform.position}");
    }
}
