using System;
using Repopulate.Player;
using TMPro;
using UnityEngine;

public class DroidCommandExecutor : CommandExecutor {
    
    private DroidControllerBase _controller;
    
    [SerializeField] private TMP_InputField _input;
    
    void Start() {
        _controller = GetComponentInParent<DroidControllerBase>();
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
