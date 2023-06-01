using UnityEngine;

public class ScrollBarHandler : MonoBehaviour {

    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject selectionBox;

    private int selectedId = 0;

    void Update() {
        float mouseScroll = Input.mouseScrollDelta.y;

        if (mouseScroll > 0) {
            ScrollUp();
        } else if (mouseScroll < 0) {
            ScrollDown();
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

    public void ScrollUp() {
        if (selectedId > 0) {
            selectedId--;
        }
        else {
            selectedId = slots.Length-1;
        }
        UpdateSelectionBox();
    }

    public void ScrollDown() {
        if (selectedId < slots.Length-1) {
            selectedId++;
        }
        else {
            selectedId = 0;
        }
        UpdateSelectionBox();
    }

    public void SelectSlot(int id) {
        if (id >= 0 && id <= slots.Length) {
            selectedId = id;
        }
        else if (id < 0) {
            selectedId = 0;
        }
        else {
            selectedId = slots.Length;
        }
    }

    private void UpdateSelectionBox() {
        selectionBox.transform.SetParent(slots[selectedId].transform, false);
    }
}
