using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotView : ViewModelBase, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private SlotTooltip _tooltip;

	private Resource _resource;
	private int _stackCount;

	public string ItemName => _resource.Name;
	public string ItemDescription => _resource.Description;
	public string StackCount => _stackCount.ToString();
	public Sprite SlotIcon => _resource.Sprite;

	public void OnPointerEnter(PointerEventData eventData) {
		if (_resource != null && _resource != ResourceRegistry.Instance.EMPTY) {
			_tooltip.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		_tooltip.gameObject.SetActive(false);
	}
	
	protected override void SetData(object value) {
		if (value is InventoryData data) {
			Debug.Log("Received inventory data!");
			_resource = data.Resource;
			_stackCount = data.StackCount;
			
			RaiseProperty(nameof(ItemName), ItemName);
			RaiseProperty(nameof(ItemDescription), ItemDescription);
			RaiseProperty(nameof(StackCount), StackCount);
			RaiseProperty(nameof(SlotIcon), _resource.Sprite);
		}
		else {
			Debug.Log($"wasn't inventory data. It was {value.GetType()}");
		}

		//_tooltip.SetInfo(_resource, _stackCount);
		Debug.Log($"Refreshed slot, with {_resource.Name} and the sprite should be {_resource.Sprite.name}");
	}
}
