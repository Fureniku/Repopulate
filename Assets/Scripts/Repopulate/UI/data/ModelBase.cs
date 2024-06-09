using UnityEngine;

public class ModelBase : MonoBehaviour
{
    protected void RaiseProperty(string propName, object value) {
        PropertyMediator.Instance.UpdateProperty(propName, value, transform);
    }
}
