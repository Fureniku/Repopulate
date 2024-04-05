using System;
using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject {
    
    [Header("Resource Information")]
    [SerializeField] private string _resourceName;
    [SerializeField] private string _description;
    [SerializeField] private Category _category;
    [SerializeField] private Sprite _icon;

    [Header("Inventory Sizes")]
    [SerializeField] private bool _stackable;
    [SerializeField] private int _sizeMin;
    [SerializeField] private int _sizeSmall;
    [SerializeField] private int _sizeMedium;
    [SerializeField] private int _sizeLarge;
    [SerializeField] private int _sizeExtraLarge;
    [SerializeField] private int _sizeMaximum;

    public string GetName() {
        return _resourceName;
    }
    
    public Category GetCategory() {
        return _category;
    }
    
    public string GetDescription() {
        return _description;
    }

    public Sprite GetSprite() {
        return _icon;
    }

    public int SlotCapacity(EnumSlotSizes size) {
        if (!_stackable) {
            return 1;
        }
        switch (size) {
            case EnumSlotSizes.MINIMUM:
                return _sizeMin;
            case EnumSlotSizes.SMALL:
                return _sizeSmall;
            case EnumSlotSizes.MEDIUM:
                return _sizeMedium;
            case EnumSlotSizes.LARGE:
                return _sizeLarge;
            case EnumSlotSizes.EXTRA_LARGE:
                return _sizeExtraLarge;
            case EnumSlotSizes.MAXIMUM:
                return _sizeMaximum;
        }

        return 0;
    }
}
