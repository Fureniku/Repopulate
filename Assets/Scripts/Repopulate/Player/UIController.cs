using Repopulate.UI;
using TMPro;
using UnityEngine;

namespace Repopulate.Player {
    public class UIController : MonoBehaviour {

        [SerializeField] private DroidController _droid;
        [SerializeField] private ScrollBarHandler _scrollBarHandler;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private TMP_InputField _input;
    
        private GameObject activeUI;
        private InteractableObject interactedObject;

        public ScrollBarHandler ScrollBar => _scrollBarHandler;

        public void CloseUI() {
            _droid.IsControlActive = true;
            DestroyImmediate(activeUI);
        }

        public void OpenNewUI(GameObject ui) {
            _droid.IsControlActive = false;
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
            _input.gameObject.SetActive(visible);
        
        }
    }
}
