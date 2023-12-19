using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject {

	[Header("Item Information")]
	[SerializeField] private string itemName;
	[SerializeField] private Category category;
	
	[Header("Placement Data")]
	[SerializeField] private Vector3Int size = Vector3Int.one;
	[SerializeField] private Vector3 rotationOrigin;
	[SerializeField] private bool mustBeGrounded = false;
	[SerializeField] private bool wallMount = false;
	[SerializeField] private bool mustBeOnCeiling = false;
	[SerializeField] private bool placeable = true;
	[SerializeField] private bool destroyable = true;

	[Header("Prefab Information")]
	[SerializeField] private PlaceableObject prefab;
	[SerializeField] private Sprite icon;

	public GameObject Get() {
		if (prefab == null) {
			return new GameObject();
		}
		return prefab.gameObject;
	}

	public Sprite GetIcon() {
		return icon;
	}

	public Vector3Int GetSize() {
		return size;
	}

	public int GetX() { return size.x; }
	public int GetY() { return size.y; }
	public int GetZ() { return size.z; }

	public bool MustBeGrounded => mustBeGrounded;
	public bool WallMounted => wallMount;
	public bool MustBeOnCeiling => mustBeOnCeiling;
	public string GetItemName => itemName;
	public string GetItemUnlocalizedName => itemName;
	public bool IsPlaceable => placeable;
	public bool IsDestroyable => destroyable;
}
