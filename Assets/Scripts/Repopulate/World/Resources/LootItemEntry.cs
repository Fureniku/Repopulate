using Repopulate.ScriptableObjects;
using UnityEngine;

[System.Serializable]
public class LootItemEntry {
	
	[SerializeField] private Item _item;
	[SerializeField] private int _minQuantity;
	[SerializeField] private int _maxQuantity;
	[SerializeField] private int _weight;

	public Item Item => _item;
	public int MinQuantity => _minQuantity;
	public int MaxQuantity => _maxQuantity;
	public int Weight => _weight;
	
	public bool IsRange => MinQuantity != MaxQuantity;

	public LootItemEntry(Item item, int minQty, int weight) : this(item, minQty, minQty, weight) {}

	public LootItemEntry(Item item, int minQty, int maxQty, int weight)
	{
		_item = item;
		_minQuantity = minQty;
		_maxQuantity = maxQty;
		_weight = weight;
	}
}
