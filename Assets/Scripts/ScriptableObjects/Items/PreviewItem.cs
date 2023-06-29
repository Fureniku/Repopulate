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

	public void UpdatePreview(Camera cam) {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
			BuildingGrid targetGrid;
			PlaceableObject placeable = go.GetComponent<PlaceableObject>();
			if (hit.transform.parent.GetComponent<PlaceableObject>() != null) {
				targetGrid = hit.transform.parent.GetComponent<PlaceableObject>().GetParentGrid();
			} else {
				targetGrid = hit.transform.parent.GetComponent<BuildingGrid>();
			}
			Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
			placeable.SetRotation(targetGrid.GetPreviewRotation().y);
			transform.rotation = targetGrid.GetPlacementRotation(gridPosition, droid.GetHeldRotation());
			transform.position = targetGrid.GetPlacementPosition(gridPosition, ItemRegistry.Instance.ALGAE_FARM_1);
			canPlaceNow = targetGrid.CheckGridSpaceAvailability(gridPosition, placeable.GetItem().GetSize(), droid.GetHeldRotation());

			if (placeable.GetItem().MustBeGrounded() && gridPosition.y > 0) {
				canPlaceNow = false;
			}
			meshRenderer.material = canPlaceNow ? validPlace : invalidPlace;
		}
	}

	public bool IsPlaceable() {
		return canPlaceNow;
	}
	
    public void CombineMeshes() {
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
        
        Debug.Log("Setting material");
        meshRenderer.material = invalidPlace;
    }
}
