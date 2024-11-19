using TMPro;
using UnityEngine;

public class KeyPromptController : MonoBehaviour {
    
    [SerializeField] private TMP_Text _keyText;
    [SerializeField] private TMP_Text _promptText;
    
    public void SetData(bool isActive, string keyText = "", string promptText = "") {
        if (isActive) {
            _keyText.text = keyText;
            _promptText.text = promptText;
        }
    }
}