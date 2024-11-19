using System.Collections.Generic;
using Repopulate.World.Resources;
using UnityEngine;

public class LootTable : MonoBehaviour {
    
    [SerializeField] private List<LootItemEntry> _lootItems;
    private int _totalWeighting;
    private int _entryCount;

    public int TableSize => _entryCount;
    public int TableMaxWeight => _totalWeighting;

    public void AddToList(List<LootItemEntry> newEntries) {
        _lootItems.AddRange(newEntries);
        RecalculateWeighting();
    }

    public void AddToList(LootItemEntry newEntry) {
        _lootItems.Add(newEntry);
        RecalculateWeighting();
    }

    void Awake() {
        RecalculateWeighting();
    }

    private void RecalculateWeighting() {
        _totalWeighting = 0;
        _entryCount = _lootItems.Count;
        for (int i = 0; i < _entryCount; i++) {
            _totalWeighting += _lootItems[i].Weight;
        }
    }

    public ItemStack GetRandomLoot() {
        int randomWeight = Random.Range(0, _totalWeighting);
        Debug.Log($"Loot: Generating between 0 and {_totalWeighting} across {_entryCount} entries");
        int cumulativeWeight = 0;
        for (int i = 0; i < _entryCount; i++) {
            LootItemEntry item = _lootItems[i];
            Debug.Log($"Entry {i} is {item.Item.name} with weight {item.Weight}");
            cumulativeWeight += item.Weight;

            if (randomWeight < cumulativeWeight)
            {
                int quantity = item.IsRange ? Random.Range(item.MinQuantity, item.MaxQuantity + 1) : item.MinQuantity;
                Debug.Log($"Loot: Success! Generating {quantity} x {item.Item.Name}");
                return new ItemStack(item.Item, quantity);
            }
        }
        Debug.Log($"Loot: Generating failed.");
        return null;
    }
}
