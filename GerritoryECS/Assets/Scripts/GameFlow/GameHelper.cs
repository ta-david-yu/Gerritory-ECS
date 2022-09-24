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

[System.Serializable]
public struct TryKillResult
{
	public bool Success;
}

public static class GameHelper
{
	public static bool IsTileAtPositionMoveableTo(this Contexts contexts, Vector2Int position)
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

		return true;
	}

	public static bool IsTileAtPositionOccupied(this Contexts contexts, Vector2Int position)
	{
		HashSet<GameEntity> onTileEntities = contexts.Game.GetEntitiesWithOnTilePosition(position);
		bool isEntityOccupyingTheGiveTile = onTileEntities.Any(entity => !entity.HasMoveOnTile || entity.HasMoveOnTileBegin);
		if (isEntityOccupyingTheGiveTile)
		{
			// There are already more than 1 OnTileElement entity on the given tile position & not moving away.
			return true;
		}

		HashSet<GameEntity> movingToTargetPositionEntities = contexts.Game.GetEntitiesWithMoveOnTile(position);
		if (movingToTargetPositionEntities.Count > 0)
		{
			// This position has already been reserved by another MoveOnTile entity.
			return true;
		}

		return false;
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
								bool isMoveableTo = contexts.IsTileAtPositionMoveableTo(position) && !contexts.IsTileAtPositionOccupied(position);
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

	private const int k_InivinciblePriority = -1;
	private const int k_GhsotPriority = -1;
	
	public static int GetOnTileElementKillPriority(this GameEntity onTileEntity)
	{
		int priority = 0;
		if (onTileEntity.HasTileOwner)
		{
			priority += onTileEntity.TileOwner.NumberOfOwnedTiles;
		}

		// TODO: if invinicible, set priority to k_InivinciblePriority
		// ...

		// TODO: if it's a ghost, set priority to k_GhostPriority
		// ...

		return priority;
	}

	public static TryKillResult TryKill(this Contexts contexts, GameEntity onTileEntity)
	{
		if (!onTileEntity.IsCanBeDead)
		{
			Debug.LogWarning("The entity cannot be dead.");
			return new TryKillResult { Success = false };
		}

		if (onTileEntity.IsDead)
		{
			Debug.LogWarning("The entity is already dead. Cannot be killed again.");
			return new TryKillResult { Success = false };
		}

		onTileEntity.IsDead = true;

		if (onTileEntity.HasOnTilePosition)
		{
			// If the OnTileEntity is occupying a tile, be sure to remove it from the tile.

			// Remove OnTilePosition
			Vector2Int position = onTileEntity.OnTilePosition.Value;
			onTileEntity.RemoveOnTilePosition();

			// Emit global LeaveTile message.
			var leaveTileMessageEntity = contexts.Message.CreateFixedUpdateMessageEntity();
			leaveTileMessageEntity.ReplaceOnTileElementLeaveTile(onTileEntity.OnTileElement.Id, position);
			leaveTileMessageEntity.IsLeaveBecauseOfDeath = true;
		}

		return new TryKillResult { Success = true };
	}
}
