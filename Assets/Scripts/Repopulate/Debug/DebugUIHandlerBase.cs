using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DebugUIHandlerBase : MonoBehaviour
{
    [SerializeField] protected GameObject debugUIElement;
    protected List<TMP_Text> texts;
    protected Dictionary<string, TMP_Text> debugLines;

    protected TMP_Text CreateEntry(string entryName) {
        TMP_Text text = Instantiate(debugUIElement, transform).GetComponent<TMP_Text>();
        text.gameObject.name = entryName;
        return text;
    }

    protected void UpdateText(string entry, string text) {
        if (debugLines == null) {
            Debug.LogError("Debug lines null");
            return;
        }
        debugLines[entry].SetText(text);
    }

    void Start() {
        texts = new();
        debugLines = new Dictionary<string, TMP_Text>();
        SetupTexts();
        foreach (TMP_Text textComponent in texts) {
            debugLines[textComponent.name] = textComponent;
        }
    }

    void Update() {
        UpdateTexts();
    }

    protected abstract void SetupTexts();
    protected abstract void UpdateTexts();
}
