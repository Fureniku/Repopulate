using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool wasOpen = false;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private Animator anim;

    [SerializeField] private Vector3 mountPoint;
    [SerializeField] private float mountRotation;
    [SerializeField] private GameObject mountedObject;

    void Awake() {
        anim = GetComponent<Animator>();
    }
    
    public Vector3 GetMountPoint() {
        return mountPoint;
    }

    public void ToggleDoor() {
        ForceDoorState(!wasOpen);
    }

    public void ForceDoorState(bool open) {
        anim.SetBool(IsOpen, open);
        wasOpen = open;
    }

    public void CreateModule(GameObject module) {
        Transform t = transform;
        Vector3 rot = t.eulerAngles;
        mountedObject = Instantiate(module, t.position + mountPoint, Quaternion.Euler(rot.x, rot.y + mountRotation, rot.z), t);
        mountedObject.GetComponent<ModuleController>().RegisterStationDoor(this);
    }
	
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + mountPoint, 0.1f);
    }
}
