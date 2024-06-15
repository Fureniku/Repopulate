using UnityEngine;

namespace Repopulate.Rendering {
    public class Billboard : MonoBehaviour {

        private Camera cam;

        void Start() {
            cam = Camera.main;
        }

        void LateUpdate() {
            transform.LookAt(cam.transform);
            Vector3 v = transform.rotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(-90, v.y, v.z);
            transform.Rotate(0, 180, 0);
        }
    }
}
