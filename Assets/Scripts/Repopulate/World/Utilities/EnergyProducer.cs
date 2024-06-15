using UnityEngine;

namespace Repopulate.World.Utilities {
	public class EnergyProducer : ResourceBase {

		[SerializeField] private int produced;

		public int GetProducedAmount() {
			return produced;
		}

	}
}
