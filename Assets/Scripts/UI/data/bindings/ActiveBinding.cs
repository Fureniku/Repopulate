using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBinding : BindingBase {
    
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _startInactive = false;
    
    protected override void Setup() {
        if (_target == null) {
            _target = gameObject;
        }

        if (_startInactive) {
            _target.SetActive(false);
        }
    }

    protected override void SetData(object value) {
        Debug.Log($"Active binding received value [{value}]");
        if (value is bool active) {
            _target.SetActive(active);
        }
    }
}
