using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BindingBase), true)]
public class PropertyBindingLookup : Editor
{
    private SerializedProperty _viewModelProp;
    private SerializedProperty _propertyNameProp;
    
    private List<ViewModelBase> _viewModels;
    private string[] _viewModelNames;
    private string[] _propertyNames;
    private int _selectedViewModelIndex;
    private int _selectedPropertyIndex;

    private void OnEnable() {
        _viewModelProp = serializedObject.FindProperty("_viewModel");
        _propertyNameProp = serializedObject.FindProperty("_propertyName");

        _viewModels = new List<ViewModelBase>();
        Transform currentTransform = ((BindingBase)target).transform;
        
        while (currentTransform != null) {
            _viewModels.AddRange(currentTransform.GetComponents<ViewModelBase>());
            currentTransform = currentTransform.parent;
        }

        _viewModelNames = new string[_viewModels.Count];
        int viewModelCount = _viewModels.Count;
        for (int i = 0; i < viewModelCount; i++) {
            _viewModelNames[i] = _viewModels[i].GetType().Name;
        }
        
        // Set initial selection of property based on existing value
        UpdatePropertyNames();

        if (_viewModelProp.objectReferenceValue != null)
        {
            Type viewModelType = _viewModelProp.objectReferenceValue.GetType();
            PropertyInfo[] properties = viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertyNames = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                _propertyNames[i] = properties[i].Name;
                if (properties[i].Name == _propertyNameProp.stringValue)
                {
                    _selectedPropertyIndex = i;
                }
            }
        }
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Property Binding", EditorStyles.boldLabel);

        _selectedViewModelIndex = EditorGUILayout.Popup("ViewModel", _selectedViewModelIndex, _viewModelNames);
        if (_selectedViewModelIndex < _viewModels.Count) {
            _viewModelProp.objectReferenceValue = _viewModels[_selectedViewModelIndex];
            UpdatePropertyNames();
        }

        EditorGUI.BeginChangeCheck();
        _selectedPropertyIndex = EditorGUILayout.Popup("Property", _selectedPropertyIndex, _propertyNames);
        if (EditorGUI.EndChangeCheck() && _selectedPropertyIndex < _propertyNames.Length) {
            _propertyNameProp.stringValue = _propertyNames[_selectedPropertyIndex];
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdatePropertyNames() {
        if (_viewModelProp.objectReferenceValue != null) {
            Type viewModelType = _viewModelProp.objectReferenceValue.GetType();
            PropertyInfo[] properties = viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertyNames = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++) {
                _propertyNames[i] = properties[i].Name;
            }
        } else {
            _propertyNames = Array.Empty<string>();
        }
    }
}
