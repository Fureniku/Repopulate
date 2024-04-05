using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    [SerializeField] private int _slotCount;
    [SerializeField] private EnumSlotSizes _slotSize;
    [SerializeField] private GameObject _slotParent;
    [SerializeField] private GameObject _slotPrefab;
    
    private List<InventorySlot> _slots;

    private bool _initialized = false;
    private bool _isDirty = false;

    void Awake() {
        if (!_initialized) {
            for (int i = 0; i < _slotCount; i++) {
                GameObject slotObject = Instantiate(_slotPrefab, _slotParent.transform);
                InventorySlot slot = slotObject.GetComponent<InventorySlot>();
                slotObject.name = $"Slot_{i}";
                slot.SlotSize = _slotSize;
                _slots.Add(slot);

            }
            _initialized = true;
        }
    }

    void Update() {
        if (_isDirty) {
            //TODO refresh slot contents
        }
    }

    public void MarkDirty() {
        _isDirty = true;
    }
}
