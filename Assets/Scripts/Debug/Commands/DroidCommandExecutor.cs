using UnityEngine;

public class DroidCommandExecutor : CommandExecutor {
    
    private DroidController _controller;
    
    void Start() {
        _controller = GetComponentInParent<DroidController>();
    }


    protected override MonoBehaviour GetExecutorType() {
        return _controller;
    }
}
