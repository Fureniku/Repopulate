using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour {

    [SerializeField] private CharacterController character;
    
    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject UIParent;

    [SerializeField] private InteractableObject interactedObject;

    [SerializeField] private InputActionAsset inputActionAsset;
    
    void Awake() {
        character = GetComponent<CharacterController>();
    }

    public void CloseUI() {
        character.SetPlayerActive(true);
        DestroyImmediate(activeUI);
    }

    public void OpenNewUI(GameObject ui) {
        character.SetPlayerActive(false);
        DestroyImmediate(activeUI);
        activeUI = Instantiate(ui.gameObject, UIParent.transform);
        activeUI.GetComponent<DynamicInteractedUI>().SetUIController(this);
        
    }

    public void SetInteractedObject(InteractableObject go) {
        interactedObject = go;
    }
    
    public InteractableObject GetInteractedObject() {
        return interactedObject;
    }
}
