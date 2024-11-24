using System.Collections.Generic;
using Repopulate.Player;
using Repopulate.ScriptableObjects;
using Repopulate.World.Constructs;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
	public static GlobalUIManager Instance { get; private set; }

	[SerializeField] private InteractMenuController _interactMenuController;
	[SerializeField] private GameObject _crosshair;
	
	//Toast manager
	//Chatbox manager
	//Current controllable UI interface

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
		PlayerControllable.OnAimedObjectChanged += UpdateInteractPrompt;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void UpdateInteractPrompt(GameObject aimedObject) {
		if (aimedObject == null) {
			DisableKeyPrompt();
			return;
		}
		
		if (aimedObject.TryGetComponent(out PlaceableConstruct construct)) {
			if (construct.GetPlaceable().HasInteractions) {
				SetKeyPrompt(construct.GetPlaceable());
			}
			else {
				DisableKeyPrompt();
			}
		}
	}

	private void DisableKeyPrompt() {
		if (_interactMenuController == null) {
			Debug.Log("aaaaaaa");
		}
		_interactMenuController.gameObject.SetActive(false);
		_crosshair.SetActive(true);
	}

	public void SetKeyPrompt(Construct construct) {
		_crosshair.SetActive(false);
		_interactMenuController.gameObject.SetActive(true);
		_interactMenuController.SetData(construct);
	}
}
