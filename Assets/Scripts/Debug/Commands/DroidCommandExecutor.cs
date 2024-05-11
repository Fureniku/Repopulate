using System;
using TMPro;
using UnityEngine;

public class DroidCommandExecutor : CommandExecutor {
    
    private DroidController _controller;
    
    [SerializeField] private TMP_InputField _input;
    
    void Start() {
        _controller = GetComponentInParent<DroidController>();
    }

    void Awake() {
        _input.Select();
        _input.ActivateInputField();
        _input.onFocusSelectAll = true;
    }


    protected override MonoBehaviour GetExecutorType() {
        return _controller;
    }
}
