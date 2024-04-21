using UnityEngine;

public class InventorySlot : ModelBase {
	
	public EnumSlotSizes SlotSize { get; set; }
	public Resource Resource { get; set; }
	public int StackCount { get; set; }

	public InventoryData InvData { get; set; }

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
		RaiseProperty(nameof(InvData), InvData);
		Debug.Log("Raised the data?");
	}
	
	//raise a change here...
}

public class InventoryData {
	public Resource Resource { get; private set; }
	public int StackCount { get; private set; }

	public InventoryData(Resource res, int count) {
		Resource = res;
		StackCount = count;
	}
}
