using Repopulate.World.Constructs;
using UnityEngine;

namespace Repopulate.ScriptableObjects {
	[CreateAssetMenu]
	public class Construct : WorldPlaceableBase<PlaceableConstruct> {

		[Header("Item Information")]
		[SerializeField] private Category category;
	
		[Header("Placement Data")]
		[SerializeField] private bool mustBeGrounded = false;
		[SerializeField] private bool wallMount = false;
		[SerializeField] private bool mustBeOnCeiling = false;

		public bool MustBeGrounded => mustBeGrounded;
		public bool WallMounted => wallMount;
		public bool MustBeOnCeiling => mustBeOnCeiling;
	}
}
