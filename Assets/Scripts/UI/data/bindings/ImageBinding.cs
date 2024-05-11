using UnityEngine;
using UnityEngine.UI;

public class ImageBinding : BindingBase
{
    [SerializeField] private Image _img;
    
    protected override void Setup() {
        if (_img == null) {
            _img = GetComponent<Image>();
        }
    }

    protected override void SetData(object value) {
        if (value is Sprite data) {
            Debug.Log("Received the data in the image binding!!");
            _img.sprite = data;
        }
        else {
            Debug.Log($"Image binding received data, but it was wrong. It was {value.GetType()}");
        }
    }
}
