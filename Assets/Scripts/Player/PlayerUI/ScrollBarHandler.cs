using UnityEngine;

public class ScrollBarHandler : MonoBehaviour {

    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject selectionBox;
    [SerializeField] private CharacterController characterController;

    private int selectedId = 0;

    void Update() {
        float mouseScroll = Input.mouseScrollDelta.y;

        if (mouseScroll > 0) {
            SelectSlot(selectedId-1);
        } else if (mouseScroll < 0) {
            SelectSlot(selectedId+1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectSlot(5); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SelectSlot(6); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SelectSlot(7); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { SelectSlot(8); }
    }

    public void SelectSlot(int id) {
        if (id == slots.Length + 1) {
            selectedId = 0;
        } else if (id == -1) {
            selectedId = slots.Length;
        } else if (id >= 0 && id <= slots.Length) {
            selectedId = id;
        } else if (id < 0) {
            selectedId = 0;
        } else {
            selectedId = slots.Length;
        }
        UpdateSelectionBox();
    }

    private void UpdateSelectionBox() {
        selectionBox.transform.SetParent(slots[selectedId].transform, false);
        characterController.UpdateSelection();
    }

    public int GetSelectedSlot() {
        return selectedId;
    }
}
