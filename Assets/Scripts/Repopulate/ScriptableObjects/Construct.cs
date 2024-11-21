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
		[SerializeField] private bool _requireGrid = true;
		[SerializeField] private bool _placeOnShip = false;
		[SerializeField] private bool _placeOnPlanet = false;
		[SerializeField] private bool _placeInSpace = false;

		public Category Category => _category;
		public bool MustBeGrounded => _mustBeGrounded;
		public bool WallMounted => _wallMount;
		public bool MustBeOnCeiling => _mustBeOnCeiling;
		public bool RequireGrid => _requireGrid;
		public bool CanPlaceOnShip => _placeOnShip;
		public bool CanPlaceOnPlanet => _placeOnPlanet;
		public bool CanPlaceInSpace => _placeInSpace;
		
	}
}
