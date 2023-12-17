using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUtilities : MonoBehaviour
{
	public static Direction GetHitFace(Vector3 hitNormal) {
		if ((hitNormal.x != 0 && hitNormal.y != 0) || (hitNormal.x != 0 && hitNormal.z != 0) || (hitNormal.y != 0 && hitNormal.z != 0))
		{
			Debug.Log("Normal was not on a flat face!");
		}

		if (hitNormal.x < 0) return Direction.X_NEG;
		if (hitNormal.x > 0) return Direction.X_POS;
		if (hitNormal.y < 0) return Direction.Y_NEG;
		if (hitNormal.y > 0) return Direction.Y_POS;
		if (hitNormal.z < 0) return Direction.Z_NEG;
		if (hitNormal.z > 0) return Direction.Z_POS;
		
		return Direction.NONE;
	}
}

public enum Direction {
	X_POS,
	X_NEG,
	Y_POS,
	Y_NEG,
	Z_POS,
	Z_NEG,
	NONE
}