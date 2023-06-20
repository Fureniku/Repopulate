using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillController : MonoBehaviour {

    [SerializeField] private GameObject fillable;
    [SerializeField] private Image image;
    
    // Update is called once per frame
    void Update() {
        image.fillAmount = fillable.GetComponent<UIFillable>().GetProgress();

        if (image.fillAmount >= 1) {
            Destroy(transform.parent.gameObject);
        }
    }
}
