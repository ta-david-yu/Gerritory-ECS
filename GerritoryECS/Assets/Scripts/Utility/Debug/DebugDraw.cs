using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDraw
{
	public static void Cross(Vector3 position, float width, Color color, float duration)
	{
		Vector3 min = position - new Vector3(width / 2, 0, width / 2);
		Vector3 max = position + new Vector3(width / 2, 0, width / 2);

		Debug.DrawLine(min, max, color, duration);
		Debug.DrawLine(new Vector3(min.x, 0, max.z), new Vector3(max.x, 0, min.z), color, duration);
	}

	public static void Tile(Vector3 position, float width, Color color, float duration)
	{
		Vector3 min = position - new Vector3(width / 2, 0, width / 2);
		Vector3 max = position + new Vector3(width / 2, 0, width / 2);
		Vector3 minXmaxZ = new Vector3(min.x, 0, max.z);
		Vector3 maxXminZ = new Vector3(max.x, 0, min.z);

		Debug.DrawLine(min, minXmaxZ, color, duration);
		Debug.DrawLine(minXmaxZ, max, color, duration);
		Debug.DrawLine(max, maxXminZ, color, duration);
		Debug.DrawLine(maxXminZ, min, color, duration);
	}
}
