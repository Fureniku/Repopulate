using Repopulate.Player;
using Repopulate.ScriptableObjects;
using UnityEngine;

// A messsage for Future Coda!
// "Why do we have two UI controllers?"
// Because there are two different states in which to show a UI.
// First, we have this global system. Everything that is generic should be handled here - for example the tooltip when aiming at something, and the crosshair.
// Then, we have the local UI controller. That's attached to a controllable object - droid, ship, whatever. It should handle anything unique to that, such as inventories.
// The global manager doesn't have access to whatever object is currently being controlled.

public class GlobalUIManager : MonoBehaviour
{
	public static GlobalUIManager Instance { get; private set; }
	
	public bool IsInMenu { get; set; }

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
	}

	private void Start() {
		InteractionHandler.OnAimedObjectChanged += UpdateLookedAtObject;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
			InteractionHandler.OnAimedObjectChanged -= UpdateLookedAtObject;
		}
	}

	private void UpdateLookedAtObject(IInteractable interactable, PlayerControllable controllable) {
		if (interactable == null) {
			DisableKeyPrompt();
			return;
		}

		interactable.OnLookAt(controllable);
		SetKeyPrompt(interactable.GetConstruct());
	}

	private void DisableKeyPrompt() {
		_interactMenuController.gameObject.SetActive(false);
		_crosshair.SetActive(true);
	}

	private void SetKeyPrompt(Construct construct) {
		_crosshair.SetActive(false);
		_interactMenuController.gameObject.SetActive(true);
		_interactMenuController.SetData(construct);
	}

	private bool _cachedWasKeyPrompt;

	public void ToggleTargetUI(bool isVisible) {
		if (!isVisible) {
			_cachedWasKeyPrompt = _interactMenuController.gameObject.activeSelf;
			_crosshair.SetActive(false);
			_interactMenuController.gameObject.SetActive(false);
			return;
		}
		_interactMenuController.gameObject.SetActive(_cachedWasKeyPrompt);
		_crosshair.SetActive(!_cachedWasKeyPrompt);
	}
}
