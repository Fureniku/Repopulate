using TMPro;
using UnityEngine;

namespace Repopulate.UI {
    public class DynamicButton : MonoBehaviour {
    
        [SerializeField] private GameObject _objectType;
        [SerializeField] private DynamicInteractedUI _parentUI;
        [SerializeField] private TMP_Text _text;

        public GameObject GetObjectType() {
            return _objectType;
        }

        public void ButtonClicked() {
            _parentUI.CreateObject(_objectType);
        }

        public void SetParentUI(DynamicInteractedUI ui) {
            _parentUI = ui;
        }

        public void SetObject(GameObject obj) {
            _objectType = obj;
            _text.text = obj.name;
        }
    }
}
