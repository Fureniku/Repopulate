using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItem : MonoBehaviour {

	[SerializeField] private GameObject go;
	[SerializeField] private Material invalidPlace;
	[SerializeField] private Material validPlace;
	[SerializeField] private DroidController droid;
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private float placeableRange = 10f;
	
	private bool canPlaceNow = true; //Updated with the preview, changes the material shading and also allows/disallows placement.

	public void SetObject(GameObject goIn) {
		if (goIn == null) {
			Debug.LogWarning("Trying to set null object for PreviewItem");
			return;
		}
		go = goIn;
		CombineMeshes();
	}
	
	void OnEnable()  {
		ScrollBarHandler.OnScrolled += UpdateObject;
	}


	void OnDisable()  {
		ScrollBarHandler.OnScrolled -= UpdateObject;
	}

	void UpdateObject() {
		Debug.Log("Calling update object from event");
		SetObject(droid.GetHeldItem().Get());
	}
	
	public void UpdatePreview(Camera cam) {
		if (go == null) {
			return;
		}
		
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out RaycastHit hit, placeableRange, LayerMask.GetMask("BuildingGrid"))) {
			meshRenderer.enabled = true;
			BuildingGrid targetGrid = null;
			if (hit.transform.parent.GetComponent<PlaceableObject>() != null) { //TODO not ideal; won't work if there's nested children in more complex objects.
				targetGrid = hit.transform.parent.GetComponent<PlaceableObject>().GetParentGrid();
			}

			if (hit.transform.GetComponent<GridCollider>() != null) {
				targetGrid = hit.transform.GetComponent<GridCollider>().GetGrid();
			}

			if (targetGrid == null) {
				Debug.LogError($"You forgot to add a GridCollider or PlaceableObject component to {hit.transform.name}, so nothing works!!");
			}
			
			PlaceableObject placeable = go.GetComponent<PlaceableObject>();

			Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);

			transform.position = targetGrid.GetPlacementPosition(gridPosition, placeable.GetItem());
			transform.rotation = targetGrid.GetPlacementRotation(droid.GetHeldRotation());

			canPlaceNow = targetGrid.CheckGridSpaceAvailability(gridPosition, placeable.GetItem().GetSize(), droid.GetHeldRotation());

			if (placeable.GetItem().MustBeGrounded() && gridPosition.y > 0) {
				canPlaceNow = false;
			}
			meshRenderer.material = canPlaceNow ? validPlace : invalidPlace;
		} else {
			meshRenderer.enabled = false;
		}
	}

	public bool IsPlaceable() {
		return canPlaceNow;
	}
	
    private void CombineMeshes() {
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        
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
