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
public struct TryCommandKillResult
{
	public bool Success;
}

public static class GameHelper
{
	/// <summary>
	/// Check if the tile itself is moveable to or not. <see cref="OnTileElementComponent"/> is not taken into consideration.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="position"></param>
	/// <returns></returns>
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

	/// <summary>
	/// Check if the tile at the given position is occupied by any <see cref="OnTileElementComponent"/>.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public static bool IsTileAtPositionOccupied(this Contexts contexts, Vector2Int position)
	{
		HashSet<ElementEntity> onTileEntities = contexts.Element.GetEntitiesWithOnTilePosition(position);
		bool isEntityOccupyingTheGiveTile = onTileEntities.Any(entity => !entity.HasMoveOnTile || entity.HasMoveOnTileBegin);
		if (isEntityOccupyingTheGiveTile)
		{
			// There are already more than 1 OnTileElement entity on the given tile position & not moving away.
			return true;
		}

		HashSet<ElementEntity> movingToTargetPositionEntities = contexts.Element.GetEntitiesWithMoveOnTile(position);
		if (movingToTargetPositionEntities.Count > 0)
		{
			// This position has already been reserved by another MoveOnTile entity.
			return true;
		}

		return false;
	}

	private static IGroup<TileEntity> s_RespawnableTileGroup = null;
	private static TryGetValidRespawnPositionResult tryGetValidRespawnPositionFor(this Contexts contexts, ElementEntity entity)
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
			Debug.LogError($"Cannot find valid tiles for the OnTileElement entity {entity.OnTileElement.Id} to be spawned on.");
			return new TryGetValidRespawnPositionResult() { Success = false, TilePosition = Vector2Int.zero };
		}

		TileEntity randomlyPickedTileEntity = validTileEntities.ElementAt(UnityEngine.Random.Range(0, validTileEntities.Count()));
		return new TryGetValidRespawnPositionResult() { Success = true, TilePosition = randomlyPickedTileEntity.TilePosition.Value };
	}
	public static TryGetValidRespawnPositionResult TryGetValidRespawnPositionOfAreaIdFor(this Contexts contexts, ElementEntity entity, int respawnAreaId)
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

	public static int GetOnTileElementKillPriority(this Contexts contexts, ElementEntity onTileEntity)
	{
		int priority = 0;
		if (onTileEntity.IsTileOwner && onTileEntity.HasTeam)
		{
			LevelEntity teamEntity = contexts.Level.GetEntityWithTeamInfo(onTileEntity.Team.Id);
			priority += teamEntity.TeamScore.Value;
		}

		// TODO: if invinicible, set priority to k_InivinciblePriority
		// ...

		// TODO: if it's a ghost, set priority to k_GhostPriority
		// ...

		return priority;
	}

	public static int GetNumberOfTeamPlayersAlive(this ElementContext context, int teamId)
	{
		return context.GetEntitiesWithTeam(teamId).Where(gameEntity => gameEntity.HasPlayer && !gameEntity.IsDead).Count();
	}

	public static TryCommandKillResult TryCommandKill(this Contexts contexts, ElementEntity onTileEntity)
	{
		if (!onTileEntity.IsCanBeDead)
		{
			Debug.LogWarning("The entity cannot be dead.");
			return new TryCommandKillResult { Success = false };
		}

		if (onTileEntity.IsDead)
		{
			Debug.LogWarning("The entity is already dead. Cannot be killed again.");
			return new TryCommandKillResult { Success = false };
		}

		// Create mark dead request entity.
		contexts.Command.CreateEntity().AddMarkOnTileElementDead(onTileEntity.OnTileElement.Id);
		return new TryCommandKillResult { Success = true };
	}

	public static bool CanStepOnVictim(this Contexts contexts, ElementEntity stepperEntity, ElementEntity victimEntity)
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

	public static ElementEntity ConstructPlayerEntity(this Contexts contexts, int playerId, int teamId, int skinId)
	{
		IPlayerFactory playerFactory = contexts.Config.GameConfig.value.PlayerFactory;

		// Create player entity and its view controller
		ElementEntity playerEntity = contexts.Element.CreateEntity();
		IEntityCreationEventController viewController = playerFactory.CreatePlayerView(playerId, teamId, skinId);
		viewController.OnEntityCreated(playerEntity);

		// Add needed componenets
		playerEntity.AddPlayer(playerId);
		playerEntity.AddTeam(teamId);
		playerEntity.AddOnTileElement(contexts.Level.GetNewOnTileElementId());
		playerEntity.AddItemEater(contexts.Level.GetNewItemEaterId());
		playerEntity.AddStateHolder(contexts.Level.GetNewStateHolderId());
		playerEntity.AddSpeedChangeable(1, 1);
		playerEntity.IsTileOwner = true;
		playerEntity.IsTileCollapser = true;
		playerEntity.IsCanBeDead = true;
		playerEntity.IsCanRespawnAfterDeath = true;
		playerEntity.IsOnTileElementKiller = true;

		TryGetValidRespawnPositionResult result = contexts.TryGetValidRespawnPositionOfAreaIdFor(playerEntity, 0);
		if (result.Success)
		{
			playerEntity.AddOnTilePosition(result.TilePosition);
		}
		else
		{
			Debug.LogError("Cannot find a valid position to spawn the player, place the player at (0, 0)");
			playerEntity.AddOnTilePosition(Vector2Int.zero);
		}

		contexts.Message.EmitOnTileElementEnterTileMessage(playerEntity.OnTileElement.Id, playerEntity.OnTilePosition.Value);

		viewController.OnComponentsAdded(playerEntity);

		// Link view controller with entity
		viewController.Link(playerEntity);

		return playerEntity;
	}

	public static int GetNewOnTileElementId(this LevelContext context)
	{
		int id = context.OnTileElementIdCounter.value.Value;
		context.ReplaceOnTileElementIdCounter(new UniqueIdCounter { Value = id + 1 });
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

	public static TileEntity ConstructTileEntityAtPosition(this Contexts contexts, TileData tileData, Vector2Int tilePosition)
	{
		ITileFactory tileFactory = contexts.Config.GameConfig.value.TileFactory;

		string tileId = tileData.TileId;

		// Create entity and its view controller
		var tileEntity = contexts.Tile.CreateEntity();
		IEntityCreationEventController viewController = tileFactory.CreateTileView(tileId);
		viewController.OnEntityCreated(tileEntity);

		// Apply blueprint and components
		var blueprint = tileFactory.GetTileBlueprint(tileId);
		blueprint.ApplyToEntity(tileEntity);
		tileEntity.AddTilePosition(tilePosition);
		viewController.OnComponentsAdded(tileEntity);

		// Link view controller with entity
		viewController.Link(tileEntity);

		// TODO: remove this!
		if (Random.Range(0.0f, 1.0f) > 0.8f)
		{
			var itemEntity = contexts.Item.CreateEntity();
			itemEntity.AddOnTileItem(tilePosition);
			itemEntity.AddApplySpeedChangeStateForEaterOnEaten(3.0f, 2.0f);
		}

		return tileEntity;
	}

	public static float GetElementEntityMoveOnTileDuration(this ElementEntity elementEntity)
	{
		if (elementEntity.HasSpeedChangeable)
		{
			return GameConstants.MoveOnTileDuration / (elementEntity.SpeedChangeable.BaseSpeed * elementEntity.SpeedChangeable.SpeedMultiplier);
		}
		else
		{
			return GameConstants.MoveOnTileDuration;
		}
	}
}