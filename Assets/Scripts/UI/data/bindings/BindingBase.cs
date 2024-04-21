using UnityEngine;

public abstract class BindingBase : MonoBehaviour
{
	[SerializeField] protected string _propertyName;
	
	private void Start() {
		PropertyMediator.Instance.OnPropertyChanged += OnPropertyUpdated;
	}
	
	private void OnDestroy() {
		PropertyMediator.Instance.OnPropertyChanged -= OnPropertyUpdated;
	}

	private void OnPropertyUpdated(string propName, object value) {
		if (propName == _propertyName) {
			SetData(value);
		}
	}
	
	protected abstract void SetData(object value);
}
