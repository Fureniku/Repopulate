using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    [SerializeField] private int _slotCount;
    [SerializeField] private EnumSlotSizes _slotSize;
    [SerializeField] private GameObject _slotParent;
    [SerializeField] private GameObject _slotPrefab;

    private List<InventorySlot> _slots = new();

    private bool _initialized = false;
    private bool _isDirty = false;

    void Awake() {
        if (!_initialized) {
            _slots.Clear();
            for (int i = 0; i < _slotCount; i++) {
                GameObject slotObject = Instantiate(_slotPrefab, _slotParent.transform);
                InventorySlot slot = slotObject.GetComponent<InventorySlot>();
                slotObject.name = $"Slot_{i}";
                slot.SlotSize = _slotSize;
                slot.Resource = ResourceRegistry.Instance.EMPTY;
                _slots.Add(slot);

            }
            Debug.Log($"Initialized inventory with {_slots.Count} slots");
            _initialized = true;
        }
    }

    void Update() {
        if (_isDirty) {
            for (int i = 0; i < _slots.Count; i++) {
                Debug.Log($"Refreshing slot {i}");
                _slots[i].RefreshSlot();
            }

            _isDirty = false;
        }
    }

    public void MarkDirty() {
        _isDirty = true;
    }

    public bool GiveResource(Resource res, int count, bool simulate = false) {
        Debug.Log($"Attempting to give {count} x {res.Name}. There are {_slots.Count} slots available in this inventory.");
        if (_initialized) {
            Debug.Log("The inventory IS initialized. Its name is {name}");
        }
        else {
            Debug.Log($"The inventory is NOT initialized. Its name is {name}");
        }
        int remain = count;
        for (int i = 0; i < _slots.Count; i++) {
            InventorySlot slot = _slots[i];
            if (slot.Resource.ID == res.ID) {
                Debug.Log("Attempting to insert to existing slot");
                int available = slot.GetAvailableSpace();
                if (available > remain) {
                    if (!simulate) {
                        slot.PutResource(res, remain);
                    }
                    MarkDirty();
                    Debug.Log("success! increased existing stack");
                    return true;
                }
                if (available > 0) {
                    slot.PutResource(res, available);
                    remain -= available;
                }
            } else if (slot.Resource == ResourceRegistry.Instance.EMPTY) {
                
                //Clone of above, function?
                int available = res.SlotCapacity(slot.SlotSize);
                Debug.Log($"Attempting to insert to empty slot. Slot capacity is {available}");
                if (available > remain) {
                    if (!simulate) {
                        slot.PutResource(res, remain);
                    }
                    MarkDirty();
                    Debug.Log("success! placed new stack");
                    return true;
                }
                if (available > 0) {
                    slot.PutResource(res, available);
                    remain -= available;
                }
            }
        }

        if (remain != count) {
            Debug.Log("Incomplete Give used! We need to handle this edge case.");
            MarkDirty();
        }
        return false;
    }
}
