using System.Collections.Generic;
using UnityEngine;

namespace Repopulate.World.Registries {
    
    [CreateAssetMenu]
    public class ItemRegistry : RegistryBase<Item> {
        
        private static ItemRegistry _instance;
        
        public static ItemRegistry Instance {
            get {
                if (ReferenceEquals(_instance, null)) {
                    _instance = Resources.Load<ItemRegistry>("Item_Registry");
                    if (!ReferenceEquals(_instance, null)) {
                        _instance.Initialize();
                    } else {
                        Debug.LogError("No instance of Item_Registry found in Resources folder.");
                    }
                }
                return _instance;
            }
        }

        public Item EMPTY;
        public Item RAW_IRON_ORE;
        public Item RAW_GOLD_ORE;
        public Item RAW_CARBON_ORE;
        public Item RAW_COPPER_ORE;
        public Item RAW_URANIUM_ORE;

        protected override List<Item> GetList() {
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
        
        public override Item GetFromName(string resourceName) {
            for (int i = 0; i < Count; i++) {
                if (string.CompareOrdinal(List[i].UnlocalizedName, resourceName) == 0) {
                    return List[i];
                }
            }
            return null;
        }
    }
}
