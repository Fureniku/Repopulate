using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewModelBase : MonoBehaviour
{
	private Dictionary<string, object> _propertyMap = new Dictionary<string, object>();
	
	protected void RaiseProperty(string propName, object value) {
		Debug.Log($"View model {name} is raising property {propName} with value {value}");
		UpdateProperty(propName, value, transform);
	}
	
	public void UpdateProperty(string propertyName, object value, Transform origin) {
		_propertyMap[propertyName] = value;
		NotifyPropertyChanged(propertyName, value, origin);
	}

	private void NotifyPropertyChanged(string propertyName, object value, Transform origin) {
		if (OnPropertyChanged != null) {
			Debug.Log($"<color=#00FF00>Invoking OnPropertyChanged on {name} with {propertyName} as [{value}]</color>");
			OnPropertyChanged.Invoke(propertyName, value);
		}
		// Propagate the change downwards through the hierarchy
		foreach (Transform child in origin) {
			if (child != origin) {
				Debug.Log($"VM: Notifying {child} about property {propertyName} changing");
				NotifyPropertyChanged(propertyName, value, child);
			}
		}
	}

	//Maybe find a better guide on this style of property bindings?
	public event Action<string, object> OnPropertyChanged;

	protected abstract void SetData(object value);
}
