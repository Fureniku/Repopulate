using UnityEngine;

[CreateAssetMenu]
public class Scrollbar : ScriptableObject {

	[SerializeField] private Construct[] slots = new Construct[9];
	[SerializeField] private int currentSlot = 0;
	private readonly int maxSlots = 8; //Zero-based

	public Construct GetSelectedConstruct() {
		if (slots[currentSlot] == null) {
			Debug.Log("Getting null slot, defaulting to empty construct");
			return GameManager.Instance.EmptyConstruct;
		}
		Debug.Log("Getting construct in slot " + currentSlot);
		Debug.Log("Construct is " + slots[currentSlot].Get().name);
		return slots[currentSlot];
	}

	public Construct GetConstructInSlot(int slot) {
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
