using UnityEngine;

public abstract class BindingBase : MonoBehaviour
{
	[SerializeField] protected string _propertyName;
	[SerializeField] protected ViewModelBase _viewModel;
	
	private void Start() {
		Debug.Log($"<color=#FFFF00>Subscribing {name} to OnPropertyChanged listener, listening for property {_propertyName}</color>");
		_viewModel.OnPropertyChanged += OnPropertyUpdated;
		Setup();
	}
	
	private void OnDestroy() {
		_viewModel.OnPropertyChanged -= OnPropertyUpdated;
	}

	protected void OnPropertyUpdated(string propName, object value) {
		Debug.LogWarning($"{name} received property change on {propName}, listening to {_propertyName}");
		if (string.CompareOrdinal(propName, _propertyName) == 0) {
			SetData(value);
		}
	}
	
	protected abstract void SetData(object value);
	protected abstract void Setup();
}
