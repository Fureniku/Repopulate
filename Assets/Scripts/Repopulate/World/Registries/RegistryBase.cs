using System.Collections.Generic;
using UnityEngine;

namespace Repopulate.World.Registries {
	
	public abstract class RegistryBase<T> : ScriptableObject {

		public List<T> List { get; private set; }
		public int Count { get; private set; }

		protected void Initialize() {
			List = GetList();
			Count = List.Count;
		}
		
		protected abstract List<T> GetList();
		
		public T? GetFromID(int id) {
			return id < List.Count ? List[id] : default;
		}

		public abstract T GetFromName(string resourceName);
	}
}
