using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ModuleConstructUI : DynamicInteractedUI {

	public void CreateModule(ModuleButton btn) {
		GameObject module = btn.GetModuleType();
		DoorController door = uiController.GetInteractedObject().GetComponent<DoorControlPanel>().GetDoorController();
		door.CreateModule(module);
		
		uiController.CloseUI();
	}
}
