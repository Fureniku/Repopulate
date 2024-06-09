using System;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
	
	public EnumSlotSizes SlotSize { get; set; }
	public Item Item { get; set; }
	public int StackCount { get; set; }

	public InventoryData InvData;

	private InventorySlotView _view;

	void Start() {
		_view = GetComponent<InventorySlotView>();
	}

	void OnEnable() {
		_view = GetComponent<InventorySlotView>();
		TempUpdate();
	}

	//TODO lots of prototype logic for inventories! it's all client-side for now.
	public int GetAvailableSpace() {
		return Item.SlotCapacity(SlotSize) - StackCount;
	}

	public void PutItem(Item item, int count) {
		if (Item == ItemRegistry.Instance.EMPTY) {
			Item = item;
			StackCount = count;
		}
		else if (Item.ID == item.ID) {
			StackCount += count;
		}

		if (StackCount > Item.SlotCapacity(SlotSize)) {
			StackCount = Item.SlotCapacity(SlotSize);
		}

		InvData = new InventoryData(Item, StackCount, SlotSize);
		TempUpdate();
	}

	private void TempUpdate() {
		if (_view != null) {
			//RaiseProperty(nameof(InvData), InvData);
			//Debug.Log("Raised the data?");
			Debug.Log($"========= Slot {name} is going to do the temporary data setting call");
			_view.TempSetData(InvData);
		}
		else {
			Debug.Log("View for invslot was null, not updating.");
		}
	}
}

[Serializable]
public class InventoryData {
	public Item Item;
	public int StackCount;
	public EnumSlotSizes SlotSize;

	public InventoryData(Item item, int count, EnumSlotSizes slotSize) {
		Item = item;
		StackCount = count;
		SlotSize = slotSize;
	}
}
