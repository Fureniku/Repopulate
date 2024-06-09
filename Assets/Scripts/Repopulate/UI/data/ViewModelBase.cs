using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewModelBase : MonoBehaviour
{
	private Dictionary<string, object> _propertyMap = new Dictionary<string, object>();
	
	protected void RaiseProperty(string propName, object value) {
		UpdateProperty(propName, value, transform);
	}
	
	public void UpdateProperty(string propertyName, object value, Transform origin) {
		_propertyMap[propertyName] = value;
		NotifyPropertyChanged(propertyName, value, origin);
	}

	private void NotifyPropertyChanged(string propertyName, object value, Transform origin) {
		if (OnPropertyChanged != null) {
			OnPropertyChanged.Invoke(propertyName, value);
		}
		// Propagate the change downwards through the hierarchy
		foreach (Transform child in origin) {
			if (child != origin) {
				NotifyPropertyChanged(propertyName, value, child);
			}
		}
	}

	public object FindProperty(string propName) {
		return _propertyMap.TryGetValue(propName, out object property) ? property : null;
	}

	public event Action<string, object> OnPropertyChanged;

	protected abstract void SetData(object value);
}
