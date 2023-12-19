using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollBarHandler : MonoBehaviour {

    [SerializeField] private Scrollbar scrollbar;

    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject selectionBox;
    
    public delegate void ScrollAction();
    public static event ScrollAction OnScrolled;

    private int selectedId = 0;

    void Awake() {
        Debug.Log("Setting slot icon");
        UpdateSlot(0);
        UpdateSlot(1);
        UpdateSlot(2);
        UpdateSlot(3);
        UpdateSlot(4);
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

    public void UpdateSlot(int id) {
        if (slots[id] != null) {
            slots[id].GetComponent<Image>().sprite = scrollbar.GetItemInSlot(id).GetIcon();
        }
    }

    public void SelectSlot(int id) {
        if (id >= slots.Length) {
            selectedId = 0;
        } else if (id < 0) {
            selectedId = slots.Length-1;
        } else {
            selectedId = id;
        }
        UpdateSelectionBox();
        OnScrolled();
    }

    private void UpdateSelectionBox() {
        selectionBox.transform.SetParent(slots[selectedId].transform, false);
        scrollbar.SelectSlot(selectedId);
    }

    public int GetSelectedSlot() {
        return selectedId;
    }
}
