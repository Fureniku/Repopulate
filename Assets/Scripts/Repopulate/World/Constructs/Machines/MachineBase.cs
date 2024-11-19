using UnityEngine;

namespace Repopulate.World.Constructs.Machines {
	public abstract class MachineBase : MonoBehaviour {

		[SerializeField] private int _tickRate;

		private int _sinceLastTick;

		private void FixedUpdate() {
			if (_sinceLastTick >= _tickRate) {
				TickMachine();
				_sinceLastTick = 0;
			}
			else {
				_sinceLastTick++;
			}
		}

		protected abstract void TickMachine();
	}
}
