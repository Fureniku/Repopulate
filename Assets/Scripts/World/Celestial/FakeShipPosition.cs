using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeShipPosition : MonoBehaviour {

    private GridObjectPosition realShip;
    
    // Start is called before the first frame update
    void Start() {
        realShip = GameManager.Instance.GetShipController().ShipPhysicsObject().GetComponent<GridObjectPosition>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = realShip.transform.position / GameManager.Instance.GetSolarGridScale();

        pos.x += realShip.GetGridSpace().x * 2;
        pos.z += realShip.GetGridSpace().y * 2;
        
        transform.localPosition = pos;
    }
}
