using UnityEngine;

namespace Repopulate.World.Utilities {
	public class EnergyConsumer : ResourceBase {

		[SerializeField] private int consumeAmount;

		public int GetConsumeAmount() {
			return consumeAmount;
		}

	}
}

