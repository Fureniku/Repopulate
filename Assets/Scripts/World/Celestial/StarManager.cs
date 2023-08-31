using UnityEngine;

public class StarManager : MonoBehaviour {

    [SerializeField] private GameObject lightSource;
    [SerializeField] private MeshRenderer meshRenderer;
    private Transform lightTarget;
    private GridObjectPosition gop;
    
    
    void Start() {
        lightTarget = GameManager.Instance.GetShipController().gameObject.transform;
        gop = GetComponent<GridObjectPosition>();
    }

    // Update is called once per frame
    void Update() {
        lightSource.transform.LookAt(lightTarget);
        meshRenderer.enabled = gop.GetGridSpace() == GameManager.Instance.GetShipController().ShipPhysicsObject().GetComponent<GridObjectPosition>().GetGridSpace();
        //TODO use events instead of checking on update 
    }
}
