using UnityEngine;

namespace Repopulate.ScriptableObjects {
	[CreateAssetMenu]
	public class Category : ScriptableObject {

		[SerializeField] private string _categoryName;

		public string GetName() {
			return _categoryName;
		}

	}
}
