using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class BindingBase : MonoBehaviour
{
	[SerializeField] protected string _propertyName;
	[SerializeField] protected ViewModelBase _viewModel;
	
	private void Start() {
		_viewModel.OnPropertyChanged += OnPropertyUpdated;
		Setup();
	}
	
	private void OnDestroy() {
		_viewModel.OnPropertyChanged -= OnPropertyUpdated;
	}

	protected void OnPropertyUpdated(string propName, object value) {
		if (string.CompareOrdinal(propName, _propertyName) == 0) {
			SetData(value);
		}
	}

	private void OnEnable() {
		object dataObj = _viewModel.FindProperty(_propertyName);
		if (dataObj != null) {
			SetData(dataObj);
		}
	}

	protected abstract void SetData(object value);
	protected abstract void Setup();
}
