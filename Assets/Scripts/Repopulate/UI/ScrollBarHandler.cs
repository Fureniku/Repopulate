using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Scrollbar = Repopulate.ScriptableObjects.Scrollbar;

namespace Repopulate.UI {
    public class ScrollBarHandler : MonoBehaviour {

        [SerializeField] private Scrollbar scrollbar;

        [SerializeField] private GameObject[] slots;
        [SerializeField] private GameObject selectionBox;
    
        public delegate void ScrollAction();
        public static event ScrollAction OnScrolled;

        private int selectedId = 0;

        void Awake() {
            Debug.Log("Setting slot icon");
            for (int i = 0; i < slots.Length; i++) {
                UpdateSlot(i);
            }
            SelectSlot(0);
        }

        public void HandleUIInput(InputAction.CallbackContext context) {
            if (context.action.name.Contains("HotBar")) {
                HandleHotbarKey(context);
                return;
            }

            switch (context.action.name) {
                case "ScrollBar":
                    HandleScroll(context);
                    return;
            }
        }

        public void HandleScroll(InputAction.CallbackContext context) {
            if (context.started) {
                float scroll = context.ReadValue<Vector2>().y;
            
                if (scroll > 0) {
                    SelectSlot(selectedId-1);
                } else if (scroll < 0) {
                    SelectSlot(selectedId+1);
                }
            }
        }
    
        public void HandleHotbarKey(InputAction.CallbackContext context) {
            if (context.started) {
                int slotIndex = context.action.name switch
                {
                    "HotBar1" => 0,
                    "HotBar2" => 1,
                    "HotBar3" => 2,
                    "HotBar4" => 3,
                    "HotBar5" => 4,
                    "HotBar6" => 5,
                    "HotBar7" => 6,
                    "HotBar8" => 7,
                    "HotBar9" => 8,
                    // Add more cases if needed
                    _ => -1 // Default case, if none of the above matches
                };

                if (slotIndex != -1) {
                    SelectSlot(slotIndex);
                }
            }
        }

        private void UpdateSlot(int id) {
            if (slots[id] != null) {
                if (scrollbar.GetConstructInSlot(id) != null) {
                    slots[id].GetComponent<Image>().sprite = scrollbar.GetConstructInSlot(id).GetIcon();
                }
            }
        }

        private void SelectSlot(int id) {
            if (id >= slots.Length) {
                selectedId = 0;
            } else if (id < 0) {
                selectedId = slots.Length-1;
            } else {
                selectedId = id;
            }
            UpdateSelectionBox();
            if (OnScrolled != null) OnScrolled();
        }

        private void UpdateSelectionBox() {
            selectionBox.transform.SetParent(slots[selectedId].transform, false);
            scrollbar.SelectSlot(selectedId);
        }

        public int GetSelectedSlot() {
            return selectedId;
        }
    }
}
