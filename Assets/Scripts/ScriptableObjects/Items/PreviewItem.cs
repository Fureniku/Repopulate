using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItem : MonoBehaviour {

	[SerializeField] private GameObject go;
	[SerializeField] private Material invalidPlace;
	[SerializeField] private Material validPlace;
	[SerializeField] private DroidController droid;
	[SerializeField] private MeshRenderer meshRenderer;
	
	private bool canPlaceNow = true; //Updated with the preview, changes the material shading and also allows/disallows placement.

	public void SetObject(GameObject goIn) {
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
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
			meshRenderer.enabled = true;
			if (hit.transform.GetComponent<GridCollider>() == null) {
				Debug.LogError($"You forgot to add a GridCollider to {hit.transform.name}, so nothing works!!");
				return;
			}
			BuildingGrid targetGrid = hit.transform.GetComponent<GridCollider>().GetGrid();
			PlaceableObject placeable = go.GetComponent<PlaceableObject>();

			Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
			
			//Debug.Log($"Ray hit position: {hit.point}, returned grid position: {gridPosition}");
			//placeable.SetRotation(targetGrid.GetPreviewRotation());
			transform.position = targetGrid.GetPlacementPosition(gridPosition, placeable.GetItem());
			transform.rotation = targetGrid.GetPlacementRotation(gridPosition, droid.GetHeldRotation());

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
