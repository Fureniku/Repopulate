using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControlPanel : InteractableObject {

	[SerializeField] private DoorController doorController;
	private UIController uiController;

	void Awake() {
		uiController = FindObjectOfType<UIController>();
	}

	public override void OnInteract() {
		doorController.ToggleDoor();
	}

	public DoorController GetDoorController() {
		return doorController;
	}

	private void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			uiController.OpenNewUI();
			uiController.SetInteractedObject(this);
		}
	}
}
