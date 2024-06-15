using UnityEngine;

namespace Repopulate.World.Utilities {
	public class OxygenProducer : MonoBehaviour {

		[SerializeField] private float producedAmount;

		public float GetProducedAmount() {
			return producedAmount;
		}

	}
}
