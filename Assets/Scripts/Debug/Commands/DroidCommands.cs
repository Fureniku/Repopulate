using UnityEngine;

public class DroidCommands
{
	public static void Give(DroidController droid, string[] args) {
		string item = args[0];
		int count = 1;
		if (args.Length >= 2) {
			int.TryParse(args[1], out count);
		}

		if (item.Length == 0) {
			item = "raw_iron_ore";
		}
		
		Resource given = int.TryParse(item, out int id) ? ResourceRegistry.Instance.GetFromID(id) : ResourceRegistry.Instance.GetFromName(item);

		if (given != null) {
			droid.Give(given, count);
			Debug.Log($"Given {count} x {given.Name}");
		}
		else {
			Debug.Log($"Unknown item {item}");
		}
	}
}
