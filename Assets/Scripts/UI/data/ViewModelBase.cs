using UnityEngine;

public abstract class ViewModelBase : BindingBase
{
	protected void RaiseProperty(string propName, object value) {
		Debug.Log($"View model is raising property {propName} with value {value}");
		PropertyMediator.Instance.UpdateProperty(propName, value, transform);
	}
}
