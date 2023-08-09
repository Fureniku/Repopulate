using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemRegistry : ScriptableObject {
    
    private static ItemRegistry instance;
    
    public static ItemRegistry Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<ItemRegistry>("Item_Registry");
                if (instance == null) {
                    Debug.LogError("No instance of Item_Registry found in Resources folder.");
                }
            }
            return instance;
        }
    }

    public Item ALGAE_FARM_1;
    public Item PILLAR_1;
    public Item BATTERY_1;
    public Item CUBE_PANELS;
    public Item CUBE_METAL;

    public List<Item> GetItemList() {
        List<Item> itemList = new();

        itemList.Add(ALGAE_FARM_1);
        itemList.Add(PILLAR_1);
        itemList.Add(BATTERY_1);
        itemList.Add(CUBE_PANELS);
        itemList.Add(CUBE_METAL);
        
        return itemList;
    }
}
