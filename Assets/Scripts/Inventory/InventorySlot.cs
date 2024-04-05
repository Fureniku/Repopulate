using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	
	[SerializeField] private SlotTooltip _tooltip;
	[SerializeField] private Resource _storedResource;
	[SerializeField] private int _stackCount;
	[SerializeField] private TMP_Text _count;
	[SerializeField] private Image _icon;
	
	public EnumSlotSizes SlotSize { get; set; }

	public void OnPointerEnter(PointerEventData eventData) {
		if (_storedResource != null && _storedResource != ResourceRegistry.Instance.EMPTY) {
			_count.SetText($"{_stackCount}");
			_tooltip.SetInfo(_storedResource, _stackCount);
			_icon.sprite = _storedResource.GetSprite();
			_tooltip.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		_tooltip.gameObject.SetActive(false);
	}
	
	//TODO lots of prototype logic for inventories! it's all client-side for now.
}
