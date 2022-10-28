using System.Linq;
using Unity.Collections;
using UnityEngine;

public static partial class AIHelper
{
	private static readonly int[] s_threadSafeRandomSeedArray = new int[32]
	{
		2, 48, 87, 63, 95, 34, 22, 27, 15, 28, 42, 64, 67, 69, 57, 82, 16, 31, 3, 100, 78, 6, 75, 98, 45, 88, 4, 20, 83, 90, 21, 81
	};

	/// <summary>
	/// An old-school/thread-safe way of generating random number (from a table). Learnt it from final fantasy I.
	/// </summary>
	/// <param name="seedIndex"></param>
	/// <returns></returns>
	public static int GetRandomValueFromSeedIndex(int seedIndex)
	{
		int randomSeed = s_threadSafeRandomSeedArray[seedIndex % s_threadSafeRandomSeedArray.Length];
		return randomSeed;
	}
}
