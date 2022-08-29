using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
	public static readonly float TileOffset = 1;

	public static Vector3 TilePositionToWorldPosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * GameConstants.TileOffset;
	}
}
