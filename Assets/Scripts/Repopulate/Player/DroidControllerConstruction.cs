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
	[Tooltip("The serialized object for the scrollbar this droid uses")]
	[SerializeField] private Scrollbar _scrollbar;
	
	private float _heldRotation = 0;
	public float HeldRotation => _heldRotation;

	public Construct SelectedConstruct => _scrollbar.GetSelectedConstruct();
	
	protected override void DroidInitialized() {
		UpdateSelection();
	}

	protected override void UpdateDroidCamera() {
		
	}
	
	public override void DroidModifierInput() {
		if (IsDroidActive) {
			_heldRotation += 90f;
			if (_heldRotation > 360) _heldRotation -= 360f;
			_interactionHandler.UpdatePreview(_camera);
		}
	}

	public override void DroidDestructiveInput(InputAction.CallbackContext context) {
		
	}
	
    #region Block Placement
		private void UpdateSelection() {
			if (_scrollbar == null) {
				Debug.LogWarning($"Trying to update selection for {name} but scrollbar is null");
				return;
			}

			if (_scrollbar.GetSelectedConstruct() == null) {
				Debug.LogWarning($"Trying to update selection with {_scrollbar.name} but nothing is selected");
				return;
			}

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
					ConstructGrid targetGrid = hit.transform.GetComponent<IGridHolder>().Grid();

					if (targetGrid != null) {
						PlaceBlock(targetGrid, targetGrid.GetHitSpace(hit.point), dir);
					}
				}
			}
		}

		private void PlaceBlock(ConstructGrid targetGrid, Vector3Int gridPosition, Direction dir) {
			// Place the block
			targetGrid.TryPlaceBlock(gridPosition, _scrollbar.GetSelectedConstruct(), _heldRotation, dir);
			//}
			//Update the preview after placing a block
			_interactionHandler.UpdatePreview(_camera);
		}
		#endregion

		protected override void ControllableUpdate() {
			_interactionHandler.UpdatePreview(_camera);
		}
}
