using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
	public static readonly float TileOffset = 1;
	public static readonly Quaternion InGameCameraOrientation = new Quaternion(-0.287274003f, -0.227902189f, 0.0705751702f, -0.927660227f);

	// Placeholder value, to be replaced
	public static readonly float MoveOnTileDurationBase = 1.0f;

	public static Vector3 TilePositionToWorldPosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * GameConstants.TileOffset;
	}
}
