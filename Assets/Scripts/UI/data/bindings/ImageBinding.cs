using UnityEngine;
using UnityEngine.UI;

public class ImageBinding : MonoBehaviour
{
    [SerializeField] private string propertyName;
    [SerializeField] private Image _img;

    private void Start() {
        PropertyMediator.Instance.OnPropertyChanged += OnPropertyUpdated;
    }

    private void OnPropertyUpdated(string propName, object value) {
        if (propName == propertyName && value is InventoryData data) {
            Debug.Log("Received the data in the image binding!!");
            _img.sprite = data.Resource.Sprite;
        }
    }

    private void OnDestroy() {
        PropertyMediator.Instance.OnPropertyChanged -= OnPropertyUpdated;
    }
}
