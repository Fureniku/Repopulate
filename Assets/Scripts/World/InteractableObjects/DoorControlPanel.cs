using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControlPanel : InteractableObject {

	[SerializeField] private DoorController doorController;

	public override void OnInteract() {
		doorController.ToggleDoor();
	}

	public DoorController GetDoorController() {
		return doorController;
	}
}
