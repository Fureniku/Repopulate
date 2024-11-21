using Repopulate.ScriptableObjects;
using TMPro;
using UnityEngine;

public class KeyPromptController : MonoBehaviour {
    
    [SerializeField] private TMP_Text _keyText;
    [SerializeField] private TMP_Text _promptText;
    
    public void SetData(InteractionSet interaction) {
	    _keyText.text = interaction.KeyCode;
	    _promptText.text = interaction.PromptText;
    }
}