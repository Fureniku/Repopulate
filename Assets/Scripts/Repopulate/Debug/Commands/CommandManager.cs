using System;
using System.Collections.Generic;
using Repopulate.Player;
using UnityEngine;

public class CommandManager : MonoBehaviour {

    public void Awake() {
        if (_droidCommands == null) {
            Setup();
        }
    }
    
    private static Dictionary<string, Action<DroidController, string[]>> _droidCommands;

    private static void Setup() {
        Debug.Log("Setting up");
        _droidCommands = new() {
            { "give", DroidCommands.Give }
        };
    }

    public static void HandleCommand(object source, string command, string[] args = null) {
        if (args == null) {
            Debug.Log($"Handling command {command} from {source.GetType()} with no args");
        }
        else {
            Debug.Log($"Handling command {command} from {source.GetType()} with args {args}");
        }
        
        switch (source) {
            case DroidController droid:
                Debug.Log("Handling droid command");
                _droidCommands[command](droid, args);
                break;
        }
    }
}
