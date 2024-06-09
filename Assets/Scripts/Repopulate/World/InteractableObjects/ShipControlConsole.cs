using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControlConsole : InteractableObject {

    protected override void OnInteract() {
        Debug.Log("Interact!");
        GameManager.Instance.SwitchCamera();
    }
}
