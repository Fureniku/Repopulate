using UnityEngine;
using UnityEngine.InputSystem;

namespace Repopulate.UI {
    public class FollowMouse : MonoBehaviour {
        [SerializeField] private bool _shouldFollowMouse = true;

        void Update() {
            if (_shouldFollowMouse) {
                transform.position = Mouse.current.position.value;
            }
        }
    }
}
