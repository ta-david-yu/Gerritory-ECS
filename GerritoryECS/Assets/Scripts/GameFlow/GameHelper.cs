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
	
	public static int GetOnTileElementKillPriority(this Contexts contexts, GameEntity onTileEntity)
	{
		int priority = 0;
		if (onTileEntity.IsTileOwner && onTileEntity.HasTeam)
		{
			LevelEntity teamEntity = contexts.Level.GetEntityWithTeamInfo(onTileEntity.Team.Id);
			priority += teamEntity.TeamInfo.NumberOfOwnedTile;
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
			Vector2Int position = onTileEntity.OnTilePosition.Value;
			onTileEntity.RemoveOnTilePosition();

			if (!onTileEntity.HasMoveOnTile)
			{
				// Emit global LeaveTile message if the killed entity was not moving away from its tile.
				var leaveTileMessageEntity = contexts.Message.CreateFixedUpdateMessageEntity();
				leaveTileMessageEntity.ReplaceOnTileElementLeaveTile(onTileEntity.OnTileElement.Id, position);
				leaveTileMessageEntity.IsLeaveBecauseOfDeath = true;
			}
		}

		if (onTileEntity.HasMoveOnTile)
		{
			// If the OnTileEntity is moving, be sure to cancel the movement.
			Vector2Int fromPosition = onTileEntity.MoveOnTile.FromPosition;
			Vector2Int toPosition = onTileEntity.MoveOnTile.ToPosition;
			onTileEntity.RemoveMoveOnTile();
			onTileEntity.AddMoveOnTileEnd(fromPosition, toPosition);
		}

		return new TryKillResult { Success = true };
	}

	public static bool CanStepOnVictim(this Contexts contexts, GameEntity stepperEntity, GameEntity victimEntity)
	{
		if (stepperEntity.HasTeam && victimEntity.HasTeam && stepperEntity.Team.Id == victimEntity.Team.Id)
		{
			// Both entities are on the same team, cannot be killed.
			return false;
		}

		int stepperPriority = contexts.GetOnTileElementKillPriority(stepperEntity);
		int victimPriority = contexts.GetOnTileElementKillPriority(victimEntity);
		if (stepperPriority >= victimPriority)
		{
			// The victim entity is stronger / has higher priority, cannot be killed.
			return false;
		}

		return true;
	}

	public static int GetNewOnTileElementId(this LevelContext context)
	{
		int id = context.OnTileElementIdCounter.value.Value;
		context.ReplaceOnTileElementIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewTileOwnerId(this LevelContext context)
	{
		int id = context.TileOwnerIdCounter.value.Value;
		context.ReplaceTileOwnerIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewItemEaterId(this LevelContext context)
	{
		int id = context.ItemEaterIdCounter.value.Value;
		context.ReplaceItemEaterIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewStateHolderId(this LevelContext context)
	{
		int id = context.StateHolderIdCounter.value.Value;
		context.ReplaceStateHolderIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}
}
