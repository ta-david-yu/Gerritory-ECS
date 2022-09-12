using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JCMG.EntitasRedux;

[System.Serializable]
public struct TryGetValidRespawnPositionResult
{
	public bool Success;
	public Vector2Int TilePosition;
}

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

	private static IGroup<TileEntity> s_RespawnableTileGroup = null;
	private static TryGetValidRespawnPositionResult tryGetValidRespawnPositionFor(this Contexts contexts, GameEntity entity)
	{
		if (s_RespawnableTileGroup == null)
		{
			s_RespawnableTileGroup = 
				contexts.Tile.GetGroup(TileMatcher.AllOf(TileMatcher.TilePosition, TileMatcher.Enterable, TileMatcher.CanBeRespawnedOn));
		}

		var validTileEntities = s_RespawnableTileGroup.GetEntities().Where(
							tileEntity =>
							{
								Vector2Int position = tileEntity.TilePosition.Value;
								bool isMoveableTo = contexts.IsPositionMoveableTo(position);
								bool hasItem = contexts.Item.GetEntityWithOnTileItem(position) != null;

								// Only tiles that have nothing on it && can be moved to are valid.
								return isMoveableTo && !hasItem;
							});

		if (!validTileEntities.Any())
		{
			// No valid tiles available.
			Debug.LogWarning($"Cannot find valid tiles for the OnTileElement entity {entity.OnTileElement.Id} to be spawned on.");
			return new TryGetValidRespawnPositionResult() { Success = false, TilePosition = Vector2Int.zero };
		}

		TileEntity randomlyPickedTileEntity = validTileEntities.ElementAt(UnityEngine.Random.Range(0, validTileEntities.Count()));
		return new TryGetValidRespawnPositionResult() { Success = true, TilePosition = randomlyPickedTileEntity.TilePosition.Value };
	}
	public static TryGetValidRespawnPositionResult TryGetValidRespawnPositionOfAreaIdFor(this Contexts contexts, GameEntity entity, int respawnAreaId)
	{
		// TODO: Add respawn area id search filter
		// ...

		return tryGetValidRespawnPositionFor(contexts, entity);
	}


	private static readonly PlayerStateEntity[] s_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];
	public static void RemovePlayerStateFor(this PlayerStateContext context, int stateHolderId)
	{
		var playerStateEntitiesSet = context.GetEntitiesWithState(stateHolderId);
		if (playerStateEntitiesSet.Count > 1)
		{
			Debug.LogWarning($"There should only be at most 1 state targetting a state holder at the same time, but there are {playerStateEntitiesSet.Count}." +
				$"Something could be wrong!");
		}

		// We need to do copy the list because playerStateEntitiesSet is a shared HashSet that could be modified during foreach iteration, which is not allowed.
		// Thus we copy HashSet to a preallocated array to avoid heap/GC allocation, and do the destruction process with that array.
		playerStateEntitiesSet.CopyTo(s_PreallocatedPlayerStateEntitiesToBeDestroyed);
		for (int i = 0; i < playerStateEntitiesSet.Count; i++)
		{
			var playerStateEntity = s_PreallocatedPlayerStateEntitiesToBeDestroyed[i];
			playerStateEntity.Destroy();
		}

		return;
	}
}
