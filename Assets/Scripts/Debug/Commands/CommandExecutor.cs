using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class CommandExecutor : MonoBehaviour {

	public void ExecuteCommand(TMP_InputField input) {
		Debug.Log($"Executing command: {input.text}");
		string text = input.text;
		
		if (text.StartsWith("/"))
		{
			text = text.Substring(1);
		}
		string[] split = text.Split(' ');
		string cmd = split[0];
		string[] args = split.Skip(1).ToArray();
		CommandManager.HandleCommand(GetExecutorType(), cmd, args);
		input.text = "_";
	}

	protected abstract MonoBehaviour GetExecutorType();
}
