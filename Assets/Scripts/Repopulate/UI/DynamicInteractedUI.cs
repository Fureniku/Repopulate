using Repopulate.Player;
using UnityEngine;

namespace Repopulate.UI {
	public abstract class DynamicInteractedUI : MonoBehaviour {
		
		[SerializeField] protected UIController _uiController;

		public void SetUIController(UIController cont) {
			_uiController = cont;
		}

		public abstract void CreateObject(GameObject obj);

	}
}