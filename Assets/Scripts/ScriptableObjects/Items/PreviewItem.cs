using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewItem : MonoBehaviour {

	private Item _item;
	private GameObject _itemObject;
	
	[SerializeField] private Material invalidPlace;
	[SerializeField] private Material validPlace;
	[SerializeField] private DroidController droid;
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private float placeableRange = 10f;
	
	private bool canPlaceNow = true; //Updated with the preview, changes the material shading and also allows/disallows placement.

	public void SetObject(Item item) {
		if (item == null) {
			_item = GameManager.Instance.EmptyItem;
			_itemObject = _item.Get();
			return;
		}
		_item = item;
		_itemObject = _item.Get();
		CombineMeshes();
	}

	public Item GetItem() {
		return _item;
	}

	public GameObject GetObject() {
		return _itemObject;
	}
	
	void OnEnable()  {
		ScrollBarHandler.OnScrolled += UpdateObject;
	}


	void OnDisable()  {
		ScrollBarHandler.OnScrolled -= UpdateObject;
	}

	void UpdateObject() {
		Debug.Log("Calling update object from event");
		SetObject(droid.GetHeldItem());
	}
	
	public void UpdatePreview(Camera cam) {
		if (_itemObject == null || _item == GameManager.Instance.EmptyItem) {
			return;
		}
		
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		
		if (Physics.Raycast(ray, out RaycastHit hit, placeableRange, Constants.MASK_BUILDABLE)) {
			meshRenderer.enabled = true;
			BuildingGrid targetGrid = null;
			if (hit.transform.GetComponent<BuildableBase>() != null) { //TODO not ideal; won't work if there's nested children in more complex objects.
				targetGrid = hit.transform.GetComponent<BuildableBase>().GetGrid();
			} else {
				Debug.LogError($"You forgot to add a BuildableBase child component to {hit.transform.name}, so nothing works!!");
				return;
			}
			
			PlaceableObject placeable = _itemObject.GetComponent<PlaceableObject>();

			Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
			if (!targetGrid.CheckGridSpaceAvailability(gridPosition)) {
				gridPosition = targetGrid.GetOffsetGridSpace(gridPosition, ColliderUtilities.GetHitFace(hit.normal));
			}

			transform.position = targetGrid.GetPlacementPosition(gridPosition, placeable.GetItem());
			transform.rotation = targetGrid.GetPlacementRotation(droid.GetHeldRotation());

			canPlaceNow = targetGrid.CheckGridSpaceAvailability(gridPosition, placeable.GetItem().GetSize(), droid.GetHeldRotation());

			if (placeable.GetItem().MustBeGrounded && gridPosition.y > 0) {
				canPlaceNow = false;
			}
			meshRenderer.material = canPlaceNow ? validPlace : invalidPlace;
		} else {
			meshRenderer.enabled = false;
		}
	}

	public bool IsPlaceable() {
		return _item.IsPlaceable && canPlaceNow;
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
