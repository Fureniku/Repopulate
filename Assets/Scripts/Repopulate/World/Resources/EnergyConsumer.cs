using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyConsumer : ResourceBase {

	[SerializeField] private int consumeAmount;

	public int GetConsumeAmount() {
		return consumeAmount;
	}

}

