using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class DoorControlPanel : InteractableObject {

	[SerializeField] private DoorController doorController;
	private UIController uiController;
	[SerializeField] private GameObject ui;

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
			uiController.OpenNewUI(ui);
			uiController.SetInteractedObject(this);
		}
	}
}
