using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class DebugUIHandler_Ship : MonoBehaviour {

    [SerializeField] private ShipMoveController ship;
    [SerializeField] private GameObject debugUIElement;
    [SerializeField] private ShipView _shipView;

    private readonly List<TMP_Text> texts = new();
    private readonly Dictionary<string, TMP_Text> debugLines = new Dictionary<string, TMP_Text>();

    private Rigidbody shipRigidbody;
    private SolarSystemManager solarSystem;

    private float ShipSpeed => shipRigidbody.velocity.magnitude;
    private Vector3 AngularVelocity => shipRigidbody.angularVelocity;
    private Vector3 ShipPosition => ship.transform.position;
    private float DistanceToOrigin => Vector3.Distance(ship.transform.position, solarSystem.transform.position);
    private string ClosestVisibleObject => _shipView.ClosestVisibleObject == null ? "None Found" : _shipView.ClosestVisibleObject.name; 

    void Awake() {
        shipRigidbody = ship.ShipPhysicsObject().GetComponent<Rigidbody>();
        solarSystem = GameManager.Instance.GetSolarSystem();
        
        texts.Add(CreateEntry(nameof(ShipSpeed)));
        texts.Add(CreateEntry(nameof(AngularVelocity)));
        texts.Add(CreateEntry(nameof(ShipPosition)));
        texts.Add(CreateEntry(nameof(DistanceToOrigin)));
        texts.Add(CreateEntry(nameof(ClosestVisibleObject)));

        foreach (TMP_Text textComponent in texts) {
            debugLines[textComponent.name] = textComponent;
        }
    }

    void Update() {
        UpdateText(nameof(ShipSpeed), $"Speed: {ShipSpeed}");
        UpdateText(nameof(AngularVelocity), $"Angular Velocity: {AngularVelocity}");
        UpdateText(nameof(ShipPosition), $"Position: {ShipPosition}");
        UpdateText(nameof(DistanceToOrigin), $"Distance to origin: {DistanceToOrigin}");
        UpdateText(nameof(ClosestVisibleObject), $"Closest visible object: {ClosestVisibleObject}");
    }

    private TMP_Text CreateEntry(string entryName) {
        TMP_Text text = Instantiate(debugUIElement, transform).GetComponent<TMP_Text>();
        text.gameObject.name = entryName;
        return text;
    }

    private void UpdateText(string entry, string text) {
        debugLines[entry].SetText(text);
    }
}
