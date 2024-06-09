using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Category : ScriptableObject {

	[SerializeField] private string _categoryName;

	public string GetName() {
		return _categoryName;
	}

}
