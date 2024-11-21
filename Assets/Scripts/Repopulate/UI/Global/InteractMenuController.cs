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
    
    public void SetData(List<InteractionSet> interactions, string title) {
	    ClearCurrentKeys();
	    _title.SetText(title);
	    for (int i = 0; i < interactions.Count; i++) {
		    KeyPromptController key = null;
		    if (_entryPool.Count == 0) {
			    key = Instantiate(_keyPromptEntry, transform).GetComponent<KeyPromptController>();
		    }
		    else {
			    key = _entryPool[0];
			    key.gameObject.SetActive(true);
			    _entryPool.RemoveAt(0);
		    }
		    key.SetData(interactions[i]);
		    _activeEntryPool.Add(key);
	    }
    }

    public void ClearCurrentKeys() {
	    for (int i = 0; i < _activeEntryPool.Count; i++) {
		    _activeEntryPool[i].gameObject.SetActive(false);
	    }
	    _entryPool.AddRange(_activeEntryPool);
	    _activeEntryPool.Clear();
    }
}