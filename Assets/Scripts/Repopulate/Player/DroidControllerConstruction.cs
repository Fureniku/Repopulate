using Repopulate.Inventory;
using Repopulate.Player;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using Repopulate.World.Constructs;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroidControllerConstruction : DroidControllerBase
{
	[Space(20)]
	[Header("Construction Droid")]
	[Tooltip("The gameobject holding the preview functionality")]
	[SerializeField] private PreviewConstruct _previewConstruct;
	[Tooltip("The serialized object for the scrollbar this droid uses")]
	[SerializeField] private Scrollbar _scrollbar;
	
	private float _heldRotation = 0;
	public float HeldRotation => _heldRotation;

	public PreviewConstruct PreviewConstruct => _previewConstruct;
	public Construct SelectedConstruct => _scrollbar.GetSelectedConstruct();
	
	protected override void DroidAwake() {
		UpdateSelection();
	}

	protected override void UpdateDroidCamera() {
		_previewConstruct.UpdatePreview(_camera);
	}
	
	public override void DroidModifierInput() {
		if (IsDroidActive) {
			_heldRotation += 90f;
			if (_heldRotation > 360) _heldRotation -= 360f;
			_previewConstruct.UpdatePreview(_camera);
		}
	}

	public override void DroidDestructiveInput(InputAction.CallbackContext context) {
		
	}
	
    #region Block Placement
		private void UpdateSelection() {
			if (_previewConstruct == null) {
				Debug.LogWarning($"Trying to update held item for {name} but held item object is null.");
				return;
			}

			if (_scrollbar == null) {
				Debug.LogWarning($"Trying to update selection for {name} but scrollbar is null");
				return;
			}

			if (_scrollbar.GetSelectedConstruct() == null) {
				Debug.LogWarning($"Trying to update selection with {_scrollbar.name} but nothing is selected");
				return;
			}

			Debug.Log($"UpdateSelection! preview construct name; {_previewConstruct.name}");
			Debug.Log($"UpdateSelection! scrollbar name: {_scrollbar.name}");
			Debug.Log($"UpdateSelection! scrollbar selection: {_scrollbar.GetSelectedConstruct().name}");
			Debug.Log($"UpdateSelection! scrollbar item SO: {_scrollbar.GetSelectedConstruct().Get().name}");
		
			_previewConstruct.SetObject(_scrollbar.GetSelectedConstruct());
		}

		public override void DroidCreativeInput(InputAction.CallbackContext context) {
			Debug.LogWarning($"Starting placement from {transform.name} (right click triggered)");
			if (_previewConstruct.IsPlaceable()) {
				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = _camera.ScreenPointToRay(mousePosition);
				if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Constants.MASK_BUILDABLE)) {
					Debug.Log($"Attempting to place a block in the grid after clicking {hit.transform.name}");
					Direction dir = ColliderUtilities.GetHitFace(hit.normal);
					ConstructGrid targetGrid = hit.transform.GetComponent<ConstructBase>().GetGrid();

					if (targetGrid != null) {
						PlaceBlock(targetGrid, targetGrid.GetHitSpace(hit.point), dir);
					}
				}
			}
		}

		private void PlaceBlock(ConstructGrid targetGrid, Vector3Int gridPosition, Direction dir) {
			//Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
			//if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {

			//Debug.Log($"GridPos: {gridPosition.x}, {gridPosition.y}, {gridPosition.z}, name of hit object: {hit.transform.name}, exact hit: {hit.point}");

			// Check if there's already a block at the target grid position
			//bool isOccupied = targetGrid.CheckGridSpaceAvailability(gridPosition, Vector3Int.one);

			// Check if there's an adjacent block in any direction
			//bool hasAdjacentBlock = targetGrid.HasAdjacentBlock(gridPosition);

			//if (isOccupied && hasAdjacentBlock)
			{
				// Place against the side or top of an existing block
				//Vector3Int adjacentPosition = targetGrid.GetAdjacentPosition(gridPosition);
				//finalPosition = targetGrid.GridToWorldPosition(adjacentPosition) + Vector3.up;
			}

			// Place the block
			targetGrid.TryPlaceBlock(gridPosition, _scrollbar.GetSelectedConstruct(), _heldRotation, dir);
			//}
			//Update the preview after placing a block
			_previewConstruct.UpdatePreview(_camera);
		}
		#endregion
}
