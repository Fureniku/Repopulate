using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotView : ViewModelBase, IPointerEnterHandler, IPointerExitHandler
{
	private Resource _resource;
	private int _stackCount;
	private EnumSlotSizes _slotSize;

	public bool TooltipActive { get; private set; }
	public string ItemName => _resource.Name;
	public string ItemCategory => _resource.Category.GetName();
	public string ItemDescription => _resource.Description;
	public string StackCount => $"{_stackCount} / {_resource.SlotCapacity(_slotSize)}";
	public bool HasExtraInformation { get; private set; } = false;
	public string ExtraInfo => "";
	public Sprite SlotIcon => _resource.Sprite;

	public void OnPointerEnter(PointerEventData eventData) {
		if (_resource != null && _resource != ResourceRegistry.Instance.EMPTY) {
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
			_resource = data.Resource;
			_stackCount = data.StackCount;
			_slotSize = data.SlotSize;

			if (_resource != null) {
				RaiseProperty(nameof(ItemName), ItemName);
				RaiseProperty(nameof(ItemCategory), ItemCategory);
				RaiseProperty(nameof(ItemDescription), ItemDescription);
				RaiseProperty(nameof(StackCount), StackCount);
				RaiseProperty(nameof(SlotIcon), _resource.Sprite);
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
