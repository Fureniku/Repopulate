public static class Utilities {
	
	//Float comparison sometimes isn't accurate, this rounds them to 3 decimal places and compares.
	public static bool CheckFloatsEqual(float var1, float var2) {
		return (int)(var1 * 1000) == (int)(var2 * 1000);
	}
}
