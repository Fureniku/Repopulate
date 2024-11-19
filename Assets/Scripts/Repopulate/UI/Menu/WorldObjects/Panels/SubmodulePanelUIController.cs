using Repopulate.ScriptableObjects;
using Repopulate.World.Modules;
using UnityEngine;

public class SubmodulePanelUIController : MonoBehaviour {
    
    //[SerializeField] private AirlockDoorController _doorController;
    [SerializeField] private ModuleController _attachedModule = null;
    [SerializeField] private ModuleController _parentModule;
    
    public void CreateModule(Module module) {
        Debug.Log($"Creating a {module.GetUnlocalizedName}");
    }

    public void OpenDoors() {
        Debug.Log("Opening doors!");
    }

    public void CloseDoors() {
        Debug.Log("Closing doors!");
    }
}
