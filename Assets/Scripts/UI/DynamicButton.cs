using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class DynamicButton : MonoBehaviour {
    
    [SerializeField] private GameObject objectType;
    [SerializeField] private DynamicInteractedUI parentUI;
    [SerializeField] private TMP_Text text;

    public GameObject GetObjectType() {
        return objectType;
    }

    public void ButtonClicked() {
        parentUI.CreateObject(objectType);
    }

    public void SetParentUI(DynamicInteractedUI ui) {
        parentUI = ui;
    }

    public void SetObject(GameObject obj) {
        objectType = obj;
        text.text = obj.name;
    }
}
