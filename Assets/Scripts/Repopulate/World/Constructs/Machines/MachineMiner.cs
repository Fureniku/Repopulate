using Repopulate.Player;
using Repopulate.World.Resources;
using UnityEngine;

namespace Repopulate.World.Constructs.Machines {
	public class MachineMiner : MachineBase, IInteractable {
		
		[SerializeField] private PlaceableConstruct _construct;
		
		private ResourceMineable _mineable;
		private bool _canMine = false;
		private bool _initialSetup = false;
		private int _minedCount = 0;

		void Awake() {
			TryFindMineable();
		}
		
		private void TryFindMineable() {
			if (_construct.Grid() == null) {
				return;
			}
			
			if (_construct.Grid().GetConnectedDataHandler is ResourceMineable mineable) {
				_mineable = mineable;
				_canMine = true;
			}
			else {
				Debug.Log("Failed to find mineable resource!");
				_canMine = false;
			}
		}
		
		protected override void TickMachine() {
			if (!_initialSetup) {
				TryFindMineable();
				if (_canMine) {
					_initialSetup = true;
				}
			}
			
			if (_canMine) {
				ItemStack mined = _mineable.TryClaimItem();
				if (!mined.IsEmpty()) {
					_minedCount += mined.StackSize;
					Debug.Log($"Mined {mined.StackSize}x {mined.Item.Name}! total mined is now {_minedCount}");
				}
			}
		}
		
		public void OnInteract(PlayerControllable controllable) {
			Debug.Log($"Interacted! Machine has {_minedCount} resources!");
		}
	}
}
