using System;
using UnityEngine;

namespace Repopulate.Utils {
	
	[Obsolete("None of the current utilities should be in use. revisit.")]
	public static class Utilities {
	
		//Float comparison sometimes isn't accurate, this rounds them to 3 decimal places and compares.
		public static bool CheckFloatsEqual(float var1, float var2) {
			return (int)(var1 * 1000) == (int)(var2 * 1000);
		}

		//Gets a factor to uniformly scale down large numbers based on the biggest component value.
		public static int GetScaleFactor(params Vector3[] vecs) {
			float max = 0f;

			//Find the largest component value
			foreach (Vector3 vec in vecs) {
				for (int j = 0; j < 3; j++) {
					if (Math.Abs(vec[j]) > max) {
						max = Math.Abs(vec[j]);
					}
				}
			}
		
			//Find the length of this number in digits, and remove 3
			int numberOfDigits = (int)Math.Floor(Math.Log10((int)Math.Floor(max)) + 1) - 3;

			//Return the scale value as a power of 10; for all components to be divided by.
			return numberOfDigits > 1 ? (int)Math.Pow(10, numberOfDigits) : 1;
		}
	}
}
