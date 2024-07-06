using Repopulate.World.Constructs;
using UnityEngine;

namespace Repopulate.ScriptableObjects {
	[CreateAssetMenu]
	public class Construct : WorldPlaceableBase<PlaceableConstruct> {

		[Header("Construct Information")]
		[SerializeField] private Category _category;
		[SerializeField] private bool _mustBeGrounded = false;
		[SerializeField] private bool _wallMount = false;
		[SerializeField] private bool _mustBeOnCeiling = false;

		public Category Category => _category;
		public bool MustBeGrounded => _mustBeGrounded;
		public bool WallMounted => _wallMount;
		public bool MustBeOnCeiling => _mustBeOnCeiling;
	}
}
