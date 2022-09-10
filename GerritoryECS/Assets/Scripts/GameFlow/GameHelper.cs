using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameHelper
{
	public static bool IsPositionMoveableTo(this Contexts contexts, Vector2Int position)
	{
		Vector2Int levelSize = contexts.Game.Level.LevelSize;
		if (position.x < 0 || position.x >= levelSize.x || position.y < 0 || position.y >= levelSize.y)
		{
			// The give position is out of bounds of the level size, cannot move here.
			return false;
		}

		TileEntity tileEntity = contexts.Tile.GetEntityWithTilePosition(position);
		if (tileEntity == null)
		{
			// There is no tile here, cannot move here.
			return false;
		}

		if (!tileEntity.IsEnterable)
		{
			// The tile is not enterable, cannot move here.
			return false;
		}

		HashSet<GameEntity> onTileEntities = contexts.Game.GetEntitiesWithOnTilePosition(position);
		bool isEntityOccupyingTheGiveTile = onTileEntities.Any(entity => !entity.HasMoveOnTile || entity.HasMoveOnTileBegin);
		if (isEntityOccupyingTheGiveTile)
		{
			// There are already more than 1 OnTileElement entity on the given tile position & not moving away.
			return false;
		}

		HashSet<GameEntity> movingToTargetPositionEntities = contexts.Game.GetEntitiesWithMoveOnTile(position);
		if (movingToTargetPositionEntities.Count > 0)
		{
			// This position has already been reserved by another MoveOnTile entity.
			return false;
		}

		return true;
	}
}
