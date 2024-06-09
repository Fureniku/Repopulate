using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStorage : ResourceBase {

	[SerializeField] private int capacity;
	[SerializeField] private int currentEnergy;

	public int GetCapacity() {
		return capacity;
	}

	public int GetCurrentFIll() {
		return currentEnergy;
	}

	public void Fill(int amount) {
		currentEnergy += amount;
		if (currentEnergy > capacity) {
			currentEnergy = capacity;
		}
	}

	public bool Drain(int amount) {
		if (amount > currentEnergy) {
			return false;
		}

		currentEnergy -= amount;
		return true;
	}

}
