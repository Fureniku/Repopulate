using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenProducer : MonoBehaviour {

	[SerializeField] private float producedAmount;

	public float GetProducedAmount() {
		return producedAmount;
	}

}
