using UnityEngine;

namespace UI {
	public class DynamicInteractedUI : MonoBehaviour {
		
		[SerializeField] protected UIController uiController;

		public void SetUIController(UIController cont) {
			uiController = cont;
		}
		
	}
}