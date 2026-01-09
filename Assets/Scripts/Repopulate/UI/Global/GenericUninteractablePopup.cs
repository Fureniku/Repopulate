using TMPro;
using UnityEngine;

namespace Repopulate.UI.Global {
	public class GenericUninteractablePopup : MonoBehaviour {

		[SerializeField] private TMP_Text _title;
		[SerializeField] private TMP_Text _body;

		public void UpdatePopup(string title, string body) {
			_title.text = title;
			_body.text = body;
		}
	}
}