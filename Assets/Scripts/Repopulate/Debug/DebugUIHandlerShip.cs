using System.Collections.Generic;
using JetBrains.Annotations;
using Repopulate.UI;
using TMPro;
using UnityEngine;

public class DebugUIHandlerShip : DebugUIHandlerBase {

    [SerializeField] private ShipMoveController ship;
    [SerializeField] private ShipView _shipView;
    
    private Rigidbody shipRigidbody;
    private SolarSystemManager solarSystem;

    private float ShipSpeed => shipRigidbody.velocity.magnitude;
    private Vector3 AngularVelocity => shipRigidbody.angularVelocity;
    private Vector3 ShipPosition => ship.transform.position;
    private float DistanceToOrigin => Vector3.Distance(ship.transform.position, solarSystem.transform.position);
    private string ClosestVisibleObject => _shipView.ClosestVisibleObject == null ? "None Found" : _shipView.ClosestVisibleObject.name; 

    protected override void SetupTexts() {
        shipRigidbody = ship.ShipPhysicsObject().GetComponent<Rigidbody>();
        solarSystem = GameManager.Instance.GetSolarSystem();
        
        texts.Add(CreateEntry(nameof(ShipSpeed)));
        texts.Add(CreateEntry(nameof(AngularVelocity)));
        texts.Add(CreateEntry(nameof(ShipPosition)));
        texts.Add(CreateEntry(nameof(DistanceToOrigin)));
        texts.Add(CreateEntry(nameof(ClosestVisibleObject)));
    }

    protected override void UpdateTexts() {
        UpdateText(nameof(ShipSpeed), $"Speed: {ShipSpeed}");
        UpdateText(nameof(AngularVelocity), $"Angular Velocity: {AngularVelocity}");
        UpdateText(nameof(ShipPosition), $"Position: {ShipPosition}");
        UpdateText(nameof(DistanceToOrigin), $"Distance to origin: {DistanceToOrigin}");
        UpdateText(nameof(ClosestVisibleObject), $"Closest visible object: {ClosestVisibleObject}");
    }

    
}
