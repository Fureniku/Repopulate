using Repopulate.World.Resources;
using UnityEngine;

public class ResourceMineable : ResourceGatherableBase {
    
    [SerializeField] private LootTable _lootTable;

    private bool hasDoneLootTest = false;
    
    protected override void OnItemClaimed(ItemStack item) {
        ClaimItem();
    }

    protected override void OnItemRespawn() {
    }

    protected override void TickResource() {
        
    }

    public ItemStack TryClaimItem() {
        if (_canBeGathered) {
            ItemStack claimed = _lootTable.GetRandomLoot();
            OnItemClaimed(claimed);
            return claimed;
        }
        return ItemStack.EMPTY;
    }

    void Update() {
        if (!hasDoneLootTest && _lootTable.TableSize > 0) {
            for (int i = 0; i < 15; i++) {
                ItemStack item = _lootTable.GetRandomLoot();
                if (item == null) {
                    Debug.Log($"Entry {i} failed to generate");
                    continue;
                }
                Debug.Log($"{i}: Looted {item.Item.Name} x {item.StackSize}");
            }

            hasDoneLootTest = true;
        }

        if (!hasDoneLootTest) {
            Debug.Log("Table not yet initialized, waiting...");
        }
    }
}
