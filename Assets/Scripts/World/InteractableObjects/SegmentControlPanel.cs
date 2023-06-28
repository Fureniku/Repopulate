using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SegmentControlPanel : InteractableObject {

	[SerializeField] private SegmentController segmentController;
	[SerializeField] private SegmentController segmentAbove;
	
	//All segments have 4 doors. Easier to direct control them than use an array.
	[SerializeField] private DoorController door_1;
	[SerializeField] private DoorController door_2;
	[SerializeField] private DoorController door_3;
	[SerializeField] private DoorController door_4;
	
	private UIController uiController;
	[SerializeField] private GameObject ui;

	private bool doesRingExist;
	private float apertureOpenAmount;

	void Awake() {
		uiController = FindObjectOfType<UIController>();
	}

	public override void OnInteract() {}

	private void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			Debug.Log("UI: " + ui.gameObject.name);
			uiController.OpenNewUI(ui);
			uiController.SetInteractedObject(this);
		}
	}

	public void Lockdown() {
		ControlAllDoors(false);
		segmentController.SetClosedAmount(0.5f);
		if (segmentAbove != null) {
			segmentAbove.SetClosedAmount(1);
		}
	}

	public void CreateRing(GameObject ring) {
		segmentController.AddRing(ring);
		
		uiController.CloseUI();
		ControlAllDoors(true);
	}

	public void ControlAllDoors(bool open) {
		door_1.ForceDoorState(open);
		door_2.ForceDoorState(open);
		door_3.ForceDoorState(open);
		door_4.ForceDoorState(open);
	}
}
