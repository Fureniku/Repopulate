using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotTooltip : MonoBehaviour {

    [SerializeField] private TMP_Text _resourceName;
    [SerializeField] private TMP_Text _resourceCategory;
    [SerializeField] private TMP_Text _resourceDesc;
    [SerializeField] private TMP_Text _resourceCount;
    
    void Update() {
        transform.position = Mouse.current.position.value;
    }

    public void SetInfo(Resource resource, int count) {
        _resourceName.SetText(resource.Name);
        _resourceCategory.SetText(resource.Category.GetName());
        _resourceDesc.SetText(resource.Description);
        _resourceCount.SetText($"{count}");
    }
}
