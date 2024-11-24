using System.Collections.Generic;
using Repopulate.ScriptableObjects;
using TMPro;
using UnityEngine;

public class InteractMenuController : MonoBehaviour {
    
	[SerializeField] private TMP_Text _title;
    [SerializeField] private List<KeyPromptController> _keyEntries;
    [SerializeField] private GameObject _keyPromptEntry;

    private List<KeyPromptController> _entryPool = new();
    private List<KeyPromptController> _activeEntryPool = new();
    
    public void SetData(Construct construct) {
	    ClearCurrentKeys();
	    _title.SetText(construct.GetUnlocalizedName);
	    
	    SetupKey(construct.PrimaryInteractions);
	    SetupKey(construct.SecondaryInteractions);
	    SetupKey(construct.TertiaryInteractions);
    }

    private void SetupKey(InteractionSet interactions) {
	    if (interactions.InteractionType == PlaceableInteractions.None) {
		    return;
	    }
	    
	    KeyPromptController key = null;
	    if (_entryPool.Count == 0) {
		    key = Instantiate(_keyPromptEntry, transform).GetComponent<KeyPromptController>();
	    }
	    else {
		    key = _entryPool[0];
		    key.gameObject.SetActive(true);
		    _entryPool.RemoveAt(0);
	    }
	    key.SetData(interactions);
	    _activeEntryPool.Add(key);
    }

    public void ClearCurrentKeys() {
	    for (int i = 0; i < _activeEntryPool.Count; i++) {
		    _activeEntryPool[i].gameObject.SetActive(false);
	    }
	    _entryPool.AddRange(_activeEntryPool);
	    _activeEntryPool.Clear();
    }
}