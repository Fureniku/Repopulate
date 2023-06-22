using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject {

	[Header("Item Information")]
	[SerializeField] private string name;
	[SerializeField] private Category category;
	
	[Header("Placement Data")]
	[SerializeField] private Vector3Int size;
	[SerializeField] private bool mustBeGrounded = false;
	[SerializeField] private bool wallMount = false;

	[Header("Prefab Information")]
	[SerializeField] private PlaceableObject prefab;
	[SerializeField] private Sprite icon;

	public GameObject Get() {
		return prefab.gameObject;
	}

	public Sprite GetIcon() {
		return icon;
	}

	public Vector3Int GetSize() {
		return size;
	}

	public bool MustBeGrounded() {
		return mustBeGrounded;
	}

	public bool WallMounted() {
		return wallMount;
	}
}
