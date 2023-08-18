using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class ModuleConstructUI : DynamicInteractedUI {

	[SerializeField] private GameObject[] availableObjects;
	[SerializeField] private GameObject uiButtonPrefab;

	void Awake() {
		for (int i = 0; i < availableObjects.Length; i++) {
			GameObject btn = Instantiate(uiButtonPrefab, transform);
			btn.GetComponent<DynamicButton>().SetObject(availableObjects[i]);
			btn.GetComponent<DynamicButton>().SetParentUI(this);
		}
	}

	public override void CreateObject(GameObject obj) {
		Debug.LogWarning("Create object not implemented, go fix it");
		//DoorController door = uiController.GetInteractedObject().GetComponent<DoorControlPanel>().GetDoorController();
		//door.CreateModule(obj);
		
		uiController.CloseUI();
	}
}
