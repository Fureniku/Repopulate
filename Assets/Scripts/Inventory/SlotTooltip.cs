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
        _resourceName.SetText(resource.GetName());
        _resourceCategory.SetText(resource.GetCategory().GetName());
        _resourceDesc.SetText(resource.GetDescription());
        _resourceCount.SetText($"{count}");
    }
}
