using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    [SerializeField] private int _slotCount;
    [SerializeField] private EnumSlotSizes _slotSize;
    [SerializeField] private GameObject _slotParent;
    [SerializeField] private GameObject _slotPrefab;

    [SerializeField] private List<InventorySlot> _slots = new();

    private bool _initialized = false;

    void Awake() {
        if (!_initialized) {
            _slots.Clear();
            for (int i = 0; i < _slotCount; i++) {
                GameObject slotObject = Instantiate(_slotPrefab, _slotParent.transform);
                InventorySlot slot = slotObject.GetComponent<InventorySlot>();
                slotObject.name = $"Slot_{i}";
                slot.SlotSize = _slotSize;
                slot.Item = ItemRegistry.Instance.EMPTY;
                _slots.Add(slot);

            }
            Debug.Log($"Initialized inventory with {_slots.Count} slots");
            _initialized = true;
        }
    }

    public int InsertItem(Item item, int count, bool simulate = false) {
        Debug.Log($"Attempting to give {count} x {item.Name}. There are {_slots.Count} slots available in this inventory.");
        int remain = count;
        for (int i = 0; i < _slots.Count; i++) {
            InventorySlot slot = _slots[i];
            if (slot.Item.ID == item.ID) {
                Debug.Log("Attempting to insert to existing slot");
                int available = slot.GetAvailableSpace();
                if (available > remain) {
                    if (!simulate) {
                        slot.PutItem(item, remain);
                    }
                    Debug.Log("success! increased existing stack");
                    return 0;
                }
                if (available > 0) {
                    slot.PutItem(item, available);
                    remain -= available;
                }
            } else if (slot.Item == ItemRegistry.Instance.EMPTY) {
                
                //Clone of above, function?
                int available = item.SlotCapacity(slot.SlotSize);
                Debug.Log($"Attempting to insert to empty slot. Slot capacity is {available}");
                if (available > remain) {
                    if (!simulate) {
                        slot.PutItem(item, remain);
                    }
                    Debug.Log("success! placed new stack");
                    return 0;
                }
                if (available > 0) {
                    slot.PutItem(item, available);
                    remain -= available;
                }
            }
        }

        if (remain != count) {
            Debug.Log("Incomplete Give used! We need to handle this edge case.");
        }
        return remain;
    }

    int Insert(InventorySlot slot, int available, int remain, Item item, bool simulate = false) {
        Debug.Log($"Attempting to insert to slot. Slot capacity is {available}");
        if (available > remain) {
            if (!simulate) {
                slot.PutItem(item, remain);
            }
            Debug.Log("success! placed new stack");
            return 0;
        }
        if (available > 0) {
            slot.PutItem(item, available);
            remain -= available;
        }

        return remain;
    }
}
