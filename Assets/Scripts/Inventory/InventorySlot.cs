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

	//TODO lots of prototype logic for inventories! it's all client-side for now.
	public int GetAvailableSpace() {
		return Resource.SlotCapacity(SlotSize) - StackCount;
	}

	public void PutResource(Resource res, int count) {
		if (Resource == ResourceRegistry.Instance.EMPTY) {
			Debug.Log($"Inserting new stack {res.Name} into slot");
			Resource = res;
			StackCount = count;
		}
		else if (Resource.ID == res.ID) {
			Debug.Log("Increasing existing stack size");
			StackCount += count;
		}

		if (StackCount > Resource.SlotCapacity(SlotSize)) {
			StackCount = Resource.SlotCapacity(SlotSize);
		}

		InvData = new InventoryData(Resource, StackCount);
		//RaiseProperty(nameof(InvData), InvData);
		//Debug.Log("Raised the data?");
		Debug.Log($"========= Slot {name} is going to do the temporary data setting call");
		_view.TempSetData(InvData);
	}
}

[Serializable]
public class InventoryData {
	public Resource Resource;
	public int StackCount;

	public InventoryData(Resource res, int count) {
		Resource = res;
		StackCount = count;
	}
}
