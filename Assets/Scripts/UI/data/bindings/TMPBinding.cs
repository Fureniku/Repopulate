using TMPro;
using UnityEngine;

public class TMPBinding : BindingBase {
	
	[SerializeField] private TMP_Text _text;

	protected override void Setup() {
		if (_text == null) {
			_text = GetComponent<TMP_Text>();
		}
	}
	
	protected override void SetData(object value) {
		if (value is string data) {
			Debug.Log($"Text binding {_propertyName} received {data}");
			_text.text = data;
		}
	}
}
