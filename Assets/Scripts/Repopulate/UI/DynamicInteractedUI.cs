using Repopulate.Player;
using UnityEngine;

namespace UI {
	public abstract class DynamicInteractedUI : MonoBehaviour {
		
		[SerializeField] protected UIController uiController;

		public void SetUIController(UIController cont) {
			uiController = cont;
		}

		public abstract void CreateObject(GameObject obj);

	}
}