using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIHandler_Ship : MonoBehaviour {

    [SerializeField] private ShipMoveController ship;
    [SerializeField] private GameObject debugUIElement;

    private readonly List<TMP_Text> texts = new();
    private readonly Dictionary<string, TMP_Text> debugLines = new Dictionary<string, TMP_Text>();

    [SerializeField] private Rigidbody shipRigidbody;
    [SerializeField] private SolarSystemManager solarSystem;

    void Awake() {
        shipRigidbody = ship.ShipPhysicsObject().GetComponent<Rigidbody>();
        solarSystem = GameManager.Instance.GetSolarSystem();
        
        texts.Add(CreateEntry("Speed"));
        texts.Add(CreateEntry("AngularVelocity"));
        texts.Add(CreateEntry("Position"));
        texts.Add(CreateEntry("DistanceToOrigin"));

        foreach (TMP_Text textComponent in texts) {
            debugLines[textComponent.name] = textComponent;
        }
    }

    void Update() {
        debugLines["Speed"].SetText($"Speed: {shipRigidbody.velocity.magnitude}");
        debugLines["AngularVelocity"].SetText($"Angular Velocity: {shipRigidbody.angularVelocity}");
        debugLines["Position"].SetText($"Position: {ship.transform.position}");
        debugLines["DistanceToOrigin"].SetText($"Distance to origin: {Vector3.Distance(ship.transform.position, solarSystem.transform.position)}");
    }

    private TMP_Text CreateEntry(string entryName) {
        TMP_Text text = Instantiate(debugUIElement, transform).GetComponent<TMP_Text>();
        text.gameObject.name = entryName;
        return text;
    }
}
