using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ModuleConstructUI : DynamicInteractedUI {

	public void CreateModule(ModuleButton btn) {
		GameObject module = btn.GetModuleType();
		Debug.Log("Create a new " + module.name);
		Debug.Log("Interacted with: " + uiController.GetInteractedObject().name);
		DoorController door = uiController.GetInteractedObject().GetComponent<DoorControlPanel>().GetDoorController();
		Debug.Log("Attach it to " + door.gameObject.name + " at " + door.GetMountPoint());
		door.ForceDoorState(true);
		door.CreateModule(module);
		
		uiController.CloseUI();
	}

}
