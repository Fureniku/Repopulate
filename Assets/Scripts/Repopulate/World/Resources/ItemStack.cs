using Repopulate.ScriptableObjects;
using Repopulate.Utils.Registries;

namespace Repopulate.World.Resources {
    //promise i'm not copying minecraft with the class name it just makes sense
    public class ItemStack {
        
        public static readonly ItemStack EMPTY = new ItemStack(ItemRegistry.Instance.EMPTY, 0);
        
        public Item Item { get; private set; }
        public int StackSize { get; private set; }

        public ItemStack(Item item, int stackSize) {
            Item = item;
            StackSize = stackSize;
        }

        public bool IsEmpty() {
            return Item == ItemRegistry.Instance.EMPTY || StackSize <= 0;
        }

        public bool IsItemType(Item item) {
            return item.ID == Item.ID;
        }

        public bool IsItemType(ItemStack stack) {
            return IsItemType(stack.Item);
        }

        public static bool AreSameType(Item item1, Item item2) {
            return item1.ID == item2.ID;
        }

        public static bool AreSameType(ItemStack stack1, ItemStack stack2) {
            return AreSameType(stack1.Item, stack2.Item);
        }
    }
}
