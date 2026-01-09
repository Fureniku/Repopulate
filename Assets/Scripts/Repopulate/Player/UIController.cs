using Repopulate.UI;
using Repopulate.UI.Global;
using TMPro;
using UnityEngine;

namespace Repopulate.Player {
    public class UIController : MonoBehaviour {

        [SerializeField] private DroidControllerBase _droid;
        [SerializeField] private ScrollBarHandler _scrollBarHandler;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private GenericUninteractablePopup _genericPopup;
    
        private GameObject activeUI;
        private InteractableObject interactedObject;

        public ScrollBarHandler ScrollBar => _scrollBarHandler;

        public void CloseUI() {
            _droid.IsControlActive = true;
            DestroyImmediate(activeUI);
            GlobalUIManager.Instance.ToggleTargetUI(true);
        }

        public void OpenNewUI(GameObject ui) {
            OpenAnyMenu();
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
            OpenAnyMenu();
            _inventory.SetActive(visible);
            _input.gameObject.SetActive(visible);
        }

        public void OpenGenericPopup(string title, string body) {
            OpenAnyMenu();
            activeUI = Instantiate(_genericPopup.gameObject, transform);
            activeUI.GetComponent<GenericUninteractablePopup>().UpdatePopup(title, body);
        }

        private void OpenAnyMenu() {
            _droid.IsControlActive = false;
            DestroyImmediate(activeUI);
            GlobalUIManager.Instance.ToggleTargetUI(false);
        }
    }
}
