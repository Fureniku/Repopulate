using System.Collections;
using UnityEngine;

namespace Repopulate.Utils {
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get; private set; }
	
        public static IEnumerator WaitOnInstance() {
            while (Instance == null) {
                yield return null;
            }
        }
	
        protected virtual void Awake() {
            Instance = this as T;
        }
    }
}