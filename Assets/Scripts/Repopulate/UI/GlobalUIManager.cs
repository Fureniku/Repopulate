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
		InteractionHandler.OnAimedObjectChanged += UpdateLookedAtObject;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void UpdateLookedAtObject(GameObject aimedObject, PlayerControllable controllable) {
		Debug.Log("updating looked at object");
		if (aimedObject == null) {
			DisableKeyPrompt();
			return;
		}
		
		if (aimedObject.TryGetComponent(out InteractableCollider interactableCollider)) {
			Debug.Log($"Looking at {interactableCollider.name}");
			interactableCollider.GetInteractable().OnLookAt(controllable);
			SetKeyPrompt(interactableCollider.GetInteractable().GetConstruct());
		}
	}

	private void DisableKeyPrompt() {
		_interactMenuController.gameObject.SetActive(false);
		_crosshair.SetActive(true);
	}

	public void SetKeyPrompt(Construct construct) {
		_crosshair.SetActive(false);
		_interactMenuController.gameObject.SetActive(true);
		_interactMenuController.SetData(construct);
	}
}
