using Repopulate.Inventory;
using Repopulate.ScriptableObjects;
using Repopulate.Utils.Registries;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotView : ViewModelBase, IPointerEnterHandler, IPointerExitHandler
{
	private Item _item;
	private int _stackCount;
	private EnumSlotSizes _slotSize;

	public bool TooltipActive { get; private set; }
	public string ItemName => _item.Name;
	public string ItemCategory => _item.Category.GetName();
	public string ItemDescription => _item.Description;
	public string StackCount => $"{_stackCount} / {_item.SlotCapacity(_slotSize)}";
	public bool HasExtraInformation { get; private set; } = false;
	public string ExtraInfo => "";
	public Sprite SlotIcon => _item.Sprite;

	public void OnPointerEnter(PointerEventData eventData) {
		if (_item != null && _item != ItemRegistry.Instance.EMPTY) {
			TooltipActive = true;
			RaiseProperty(nameof(TooltipActive), TooltipActive);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		TooltipActive = false;
		RaiseProperty(nameof(TooltipActive), TooltipActive);
	}

	public void TempSetData(object data) {
		SetData(data);
	}
	
	protected override void SetData(object value) {
		if (value is InventoryData data) {
			Debug.Log($"Received inventory data! This object is {transform.name}");
			_item = data.Item;
			_stackCount = data.StackCount;
			_slotSize = data.SlotSize;

			if (_item != null) {
				RaiseProperty(nameof(ItemName), ItemName);
				RaiseProperty(nameof(ItemCategory), ItemCategory);
				RaiseProperty(nameof(ItemDescription), ItemDescription);
				RaiseProperty(nameof(StackCount), StackCount);
				RaiseProperty(nameof(SlotIcon), _item.Sprite);
				Debug.Log($"Raised slot data with name [{ItemName}], description [{ItemDescription}] and count [{StackCount}]. Probably an icon too.");
			}
		}
		else {
			Debug.Log($"wasn't inventory data. It was {value.GetType()}");
		}
	}
	
	private void Start() {
		OnPropertyChanged += OnPropertyUpdated;
	}
	
	private void OnDestroy() {
		OnPropertyChanged -= OnPropertyUpdated;
	}
	
	private void OnPropertyUpdated(string propName, object value) {
		//Debug.LogWarning($"Inv Slot View {name} received property change on {propName}");
		//if (propName == _propertyName) {
			//SetData(value);
		//}
	}
}
