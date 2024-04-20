using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : ViewBase, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private SlotTooltip _tooltip;
	[SerializeField] private TMP_Text _count;
	[SerializeField] private Image _icon;
	[SerializeField] private InventorySlot _slot;

	private Resource _resource;
	private int _stackCount;
	
	public void OnPointerEnter(PointerEventData eventData) {
		if (_slot.Resource != null && _slot.Resource != ResourceRegistry.Instance.EMPTY) {
			//RefreshData();
			_tooltip.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		_tooltip.gameObject.SetActive(false);
	}
	
	//called on raise change
	/*public override void RefreshData(object newValue) {
		_resource = _slot.Resource;
		_stackCount = _slot.StackCount;
		_count.SetText($"{_stackCount}");
		_tooltip.SetInfo(_resource, _stackCount);
		_icon.sprite = _resource.Sprite;
		Debug.Log($"Refreshed slot, with {_resource.Name} and the sprite should be {_resource.Sprite.name}");
	}*/
}
