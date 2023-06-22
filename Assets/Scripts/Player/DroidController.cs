using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroidController : MonoBehaviour {

	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;

	private bool isDroidActive = false; //Whether this is the currently controlled droid. Not to be confused with playeractive which locks the camera etc

	void Awake() {
		UpdateSelection();
	}

	void Update() {
		if (isDroidActive) {
			if (Input.GetKeyDown(KeyCode.R)) {
				heldItem.Rotate();
			}
		}
	}
	
	public void UpdateSelection() {
		heldItem.SetObject(scrollbar.GetHeldItem().Get());
	}

	public void UpdatePreview(Camera cam) {
		heldItem.UpdatePreview(cam);
	}

	public PreviewItem GetPreviewItem() {
		return heldItem;
	}

	public Item GetHeldItem() {
		return scrollbar.GetHeldItem();
	}

	public void SetDroidActive(bool active) {
		isDroidActive = active;
	}
}
