using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemRegistry : ScriptableObject {
    
    private static ItemRegistry _instance;
    
    public static ItemRegistry Instance {
        get {
            if (_instance == null) {
                _instance = Resources.Load<ItemRegistry>("Item_Registry");
                if (_instance == null) {
                    Debug.LogError("No instance of Item_Registry found in Resources folder.");
                    return null;
                }

                _instance.ItemList = _instance.GetResourceList();
                _instance.ItemCount = _instance.ItemList.Count;
            }
            return _instance;
        }
    }

    public List<Item> ItemList { get; private set; }
    public int ItemCount { get; private set; }

    public Item EMPTY;
    public Item RAW_IRON_ORE;
    public Item RAW_GOLD_ORE;
    public Item RAW_CARBON_ORE;
    public Item RAW_COPPER_ORE;
    public Item RAW_URANIUM_ORE;

    private List<Item> GetResourceList() {
        List<Item> _itemList = new();
        
        //These MUST be in ID order.
        _itemList.Add(EMPTY);
        _itemList.Add(RAW_IRON_ORE);
        _itemList.Add(RAW_GOLD_ORE);
        _itemList.Add(RAW_CARBON_ORE);
        _itemList.Add(RAW_COPPER_ORE);
        _itemList.Add(RAW_URANIUM_ORE);
        
        return _itemList;
    }

    public Item GetFromID(int id) {
        return id < ItemList.Count ? ItemList[id] : null;
    }

    public Item GetFromName(string resourceName) {
        for (int i = 0; i < ItemCount; i++) {
            if (String.CompareOrdinal(ItemList[i].UnlocalizedName, resourceName) == 0) {
                return ItemList[i];
            }
        }
        return null;
    }
}
