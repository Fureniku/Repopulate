using System;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMediator : MonoBehaviour {
    private static PropertyMediator _instance;
    private Dictionary<string, object> _propertyMap = new Dictionary<string, object>();

    public static PropertyMediator Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PropertyMediator>();
                if (_instance == null) {
                    GameObject go = new GameObject("PropertyMediator");
                    _instance = go.AddComponent<PropertyMediator>();
                }
            }
            return _instance;
        }
    }

    /*public void RegisterProperty(string propertyName, object value) {
        _propertyMap[propertyName] = value;
    }

    public object GetProperty(string propertyName) {
        if (_propertyMap.ContainsKey(propertyName)) {
            return _propertyMap[propertyName];
        }
        return null;
    }*/

    public void UpdateProperty(string propertyName, object value, Transform origin) {
        _propertyMap[propertyName] = value;
        Debug.Log($"Updated property {propertyName}, its now {_propertyMap[propertyName]}");
        NotifyPropertyChanged(propertyName, value, origin);
    }

    private void NotifyPropertyChanged(string propertyName, object value, Transform origin) {
        OnPropertyChanged?.Invoke(propertyName, value);
        // Propagate the change downwards through the hierarchy
        foreach (Transform child in transform) {
            if (child != origin) {
                NotifyPropertyChanged(propertyName, value, child);
            }
        }
    }

    public event Action<string, object> OnPropertyChanged;

    private void NotifyPropertyChanged(string propertyName, object value) {
        OnPropertyChanged?.Invoke(propertyName, value);
    }
}