using System;

namespace Repopulate.Inventory {
	
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
}
