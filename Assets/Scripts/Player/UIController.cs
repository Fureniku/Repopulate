using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject UIParent;
    [SerializeField] private GameObject TEMP_UI_TEST;

    [SerializeField] private GameObject interactedObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseUI();
        }
    }

    public void CloseUI() {
        DestroyImmediate(activeUI);
    }

    public void OpenNewUI() {
        DestroyImmediate(activeUI);
        activeUI = Instantiate(TEMP_UI_TEST, UIParent.transform);
        activeUI.GetComponent<DynamicInteractedUI>().SetUIController(this);
    }

    public void SetInteractedObject(GameObject go) {
        interactedObject = go;
    }
    
    public GameObject GetInteractedObject() {
        return interactedObject;
    }
}
