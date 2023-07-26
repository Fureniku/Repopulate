using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Scrollbar : ScriptableObject {

	[SerializeField] private Item[] slots = new Item[9];
	[SerializeField] private int currentSlot = 0;
	private readonly int maxSlots = 8; //Zero-based

	public Item GetHeldItem() {
		if (slots[currentSlot] == null) {
			return null;
		}
		Debug.Log("Getting item in slot " + currentSlot);
		Debug.Log("Item is " + slots[currentSlot].Get().name);
		return slots[currentSlot];
	}

	public Item GetItemInSlot(int slot) {
		return slots[ValidateSlot(slot)];
	}

	public void IncreaseSlot() {
		currentSlot++;
		if (currentSlot > maxSlots) {
			currentSlot = 0;
		} 
	}

	public void DecreaseSlot() {
		currentSlot--;
		if (currentSlot < 0) {
			currentSlot = maxSlots;
		}
	}

	public void SelectSlot(int slot) {
		currentSlot = ValidateSlot(slot);
	}

	//Check that slots are within range
	private int ValidateSlot(int slot) {
		if (slot < 0) {
			return 0;
		}
		if (slot > maxSlots) {
			return maxSlots;
		}
		return slot;
	}
}
