using System;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
	
	public EnumSlotSizes SlotSize { get; set; }
	public Resource Resource { get; set; }
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
		return Resource.SlotCapacity(SlotSize) - StackCount;
	}

	public void PutResource(Resource res, int count) {
		if (Resource == ResourceRegistry.Instance.EMPTY) {
			Resource = res;
			StackCount = count;
		}
		else if (Resource.ID == res.ID) {
			StackCount += count;
		}

		if (StackCount > Resource.SlotCapacity(SlotSize)) {
			StackCount = Resource.SlotCapacity(SlotSize);
		}

		InvData = new InventoryData(Resource, StackCount, SlotSize);
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
	public Resource Resource;
	public int StackCount;
	public EnumSlotSizes SlotSize;

	public InventoryData(Resource res, int count, EnumSlotSizes slotSize) {
		Resource = res;
		StackCount = count;
		SlotSize = slotSize;
	}
}
