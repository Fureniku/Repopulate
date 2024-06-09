using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyProducer : ResourceBase {

	[SerializeField] private int produced;

	public int GetProducedAmount() {
		return produced;
	}

}
