using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private CharacterController character;
    
    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject UIParent;
    [SerializeField] private GameObject TEMP_UI_TEST;

    [SerializeField] private InteractableObject interactedObject;
    
    // Start is called before the first frame update
    void Awake() {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseUI();
        }
    }

    public void CloseUI() {
        character.SetPlayerActive(true);
        DestroyImmediate(activeUI);
    }

    public void OpenNewUI() {
        character.SetPlayerActive(false);
        DestroyImmediate(activeUI);
        activeUI = Instantiate(TEMP_UI_TEST, UIParent.transform);
        activeUI.GetComponent<DynamicInteractedUI>().SetUIController(this);
    }

    public void SetInteractedObject(InteractableObject go) {
        interactedObject = go;
    }
    
    public InteractableObject GetInteractedObject() {
        return interactedObject;
    }
}