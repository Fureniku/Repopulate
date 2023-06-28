using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class RingConstructUI : DynamicInteractedUI {

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
		SegmentControlPanel segment = uiController.GetInteractedObject().GetComponent<SegmentControlPanel>();
		segment.CreateRing(obj);
		
		uiController.CloseUI();
	}
}
