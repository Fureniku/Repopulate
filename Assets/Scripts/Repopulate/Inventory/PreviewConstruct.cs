using Repopulate.Player;
using Repopulate.ScriptableObjects;
using Repopulate.UI;
using Repopulate.Utils;
using Repopulate.World.Constructs;
using Repopulate.Utils.Registries;
using UnityEngine;

namespace Repopulate.Inventory {
	public class PreviewConstruct : MonoBehaviour {

		private Construct _construct;
		private GameObject _itemObject;
		private PlaceableObject _placeableObject;
	
		[SerializeField] private Material invalidPlace;
		[SerializeField] private Material validPlace;
		[SerializeField] private DroidControllerConstruction droid;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private float placeableRange = 10f;
	
		private bool canPlaceNow = true; //Updated with the preview, changes the material shading and also allows/disallows placement.

		void Start() {
			if (_construct == null) {
				_construct = ConstructRegistry.Instance.EMPTY;
			}

			_itemObject = ConstructRegistry.Instance.EMPTY.Get();
		}
	
		public void SetObject(Construct construct) {
			if (construct == null) {
				_construct = GameManager.Instance.EmptyConstruct;
				_itemObject = _construct.Get();
				return;
			}
			_construct = construct;
			_itemObject = _construct.Get();
			_placeableObject = _itemObject.GetComponent<PlaceableObject>();
			CombineMeshes();
		}

		public Construct GetConstruct() {
			return _construct;
		}

		public GameObject GetObject() {
			if (_itemObject == null) {
				Debug.LogError("PreviewItem._itemObject is currently null! Fixing and returning empty.");
				_itemObject = ConstructRegistry.Instance.EMPTY.Get();
				return ConstructRegistry.Instance.EMPTY.Get();
			}
			return _itemObject;
		}
	
		void OnEnable()  {
			ScrollBarHandler.OnScrolled += UpdateObject;
		}


		void OnDisable()  {
			ScrollBarHandler.OnScrolled -= UpdateObject;
		}

		void UpdateObject() {
			SetObject(droid.SelectedConstruct);
		}
	
		public void UpdatePreview(Camera cam) {
			if (_itemObject == null || _placeableObject == null || _construct == GameManager.Instance.EmptyConstruct || _placeableObject.GetConstruct() == GameManager.Instance.EmptyConstruct) {
				return;
			}
		
			Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		
			if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, placeableRange, Constants.MASK_BUILDABLE)) {
				meshRenderer.enabled = true;
				ConstructGrid targetGrid = null;
				if (hit.transform.GetComponent<ConstructBase>() != null) { //TODO not ideal; won't work if there's nested children in more complex objects.
					targetGrid = hit.transform.GetComponent<ConstructBase>().GetGrid();
				} else {
					Debug.LogError($"You forgot to add a BuildableBase child component to {hit.transform.name}, so nothing works!!");
					return;
				}
			
				Construct placeableConstruct = _placeableObject.GetConstruct();
			
				Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
				if (!targetGrid.CheckGridSpaceAvailability(gridPosition)) {
					gridPosition = targetGrid.GetOffsetGridSpace(gridPosition, ColliderUtilities.GetHitFace(hit.normal));
				}

				transform.position = targetGrid.GetPlacementPosition(gridPosition, placeableConstruct);
				transform.rotation = targetGrid.GetPlacementRotation(droid.HeldRotation);

				canPlaceNow = targetGrid.CheckGridSpaceAvailability(gridPosition, placeableConstruct.GetSize(), droid.HeldRotation);

				if (_placeableObject.GetConstruct().MustBeGrounded && gridPosition.y > 0) {
					canPlaceNow = false;
				}
				meshRenderer.material = canPlaceNow ? validPlace : invalidPlace;
			} else {
				meshRenderer.enabled = false;
			}
		}

		public bool IsPlaceable() {
			return _construct.IsPlaceable && canPlaceNow;
		}
	
		private void CombineMeshes() {
			MeshFilter[] meshFilters = _itemObject.GetComponentsInChildren<MeshFilter>();
        
			Mesh combinedMesh = new Mesh();
        
			CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
        
			for (int i = 0; i < meshFilters.Length; i++) {
				combineInstances[i].mesh = meshFilters[i].sharedMesh;
				combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
			}
        
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null) {
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
        
			combinedMesh.CombineMeshes(combineInstances, true, true);
        
			meshFilter.sharedMesh = combinedMesh;
        
			combinedMesh.RecalculateNormals();
			combinedMesh.RecalculateBounds();
        
			meshRenderer.material = invalidPlace;
		}
	}
}
