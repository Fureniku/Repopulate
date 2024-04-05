using UI;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private DroidController droid;
    [SerializeField] private ScrollBarHandler _scrollBarHandler;
    [SerializeField] private GameObject _inventory;
    
    private GameObject activeUI;
    private InteractableObject interactedObject;

    public ScrollBarHandler ScrollBar => _scrollBarHandler;

    public void CloseUI() {
        droid.SetControlsActive(true);
        DestroyImmediate(activeUI);
    }

    public void OpenNewUI(GameObject ui) {
        droid.SetControlsActive(false);
        DestroyImmediate(activeUI);
        activeUI = Instantiate(ui.gameObject, transform);
        activeUI.GetComponent<DynamicInteractedUI>().SetUIController(this);
        
    }

    public void SetInteractedObject(InteractableObject go) {
        interactedObject = go;
    }
    
    public InteractableObject GetInteractedObject() {
        return interactedObject;
    }
    
    public void InventoryVisible(bool visible) {
        _inventory.SetActive(visible);
    }
}
