using System;
using Repopulate.ScriptableObjects;
using Repopulate.World.Resources;

namespace Repopulate.Inventory {
	
	[Serializable]
	public class InventoryData {
		public ItemStack Item;
		public EnumSlotSizes SlotSize;

		public InventoryData(ItemStack item, EnumSlotSizes slotSize) {
			Item = item;
			SlotSize = slotSize;
		}
	}
}
