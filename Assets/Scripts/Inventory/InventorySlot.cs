using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	
	[SerializeField] private SlotTooltip _tooltip;
	[SerializeField] private int _stackCount;
	[SerializeField] private TMP_Text _count;
	[SerializeField] private Image _icon;
	
	public EnumSlotSizes SlotSize { get; set; }
	public Resource Resource { get; set; }

	public void OnPointerEnter(PointerEventData eventData) {
		if (Resource != null && Resource != ResourceRegistry.Instance.EMPTY) {
			RefreshSlot();
			_tooltip.gameObject.SetActive(true);
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		_tooltip.gameObject.SetActive(false);
	}

	public void RefreshSlot() {
		_count.SetText($"{_stackCount}");
		_tooltip.SetInfo(Resource, _stackCount);
		_icon.sprite = Resource.Sprite;
		Debug.Log($"Refreshed slot, with {Resource.Name} and the sprite should be {Resource.Sprite.name}");
	}

	//TODO lots of prototype logic for inventories! it's all client-side for now.
	public int GetAvailableSpace() {
		return Resource.SlotCapacity(SlotSize) - _stackCount;
	}

	public void PutResource(Resource res, int count) {
		if (Resource == ResourceRegistry.Instance.EMPTY) {
			Debug.Log($"Inserting new stack {res.Name} into slot");
			Resource = res;
			_stackCount = count;
		}
		else if (Resource.ID == res.ID) {
			Debug.Log("Increasing existing stack size");
			_stackCount += count;
		}

		if (_stackCount > Resource.SlotCapacity(SlotSize)) {
			_stackCount = Resource.SlotCapacity(SlotSize);
		}
	}
}
