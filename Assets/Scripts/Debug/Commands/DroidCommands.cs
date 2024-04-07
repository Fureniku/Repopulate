using UnityEngine;

public class DroidCommands
{
	public static void Give(DroidController droid, string[] args) {
		Debug.Log("Give command executed!");
		droid.Give(ResourceRegistry.Instance.RAW_IRON_ORE, 10);
	}

}
