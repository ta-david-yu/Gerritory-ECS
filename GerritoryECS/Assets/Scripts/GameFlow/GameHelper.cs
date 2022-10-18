using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JCMG.EntitasRedux;
using TMPro;

[System.Serializable]
public struct TryGetValidRespawnPositionResult
{
	public bool Success;
	public Vector2Int TilePosition;
}

public struct TryAddPlayerStateTypeResult
{
	public bool Success;
	public PlayerStateEntity PlayerStateEntity;
}

[System.Serializable]
public struct TryCommandKillResult
{
	public bool Success;
}

public struct TryCommandSpawnItemResult
{
	public bool Success;
	public CommandEntity CommandEntity;
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
	public static void RemovePlayerStateFor(this Contexts contexts, int stateHolderId)
	{
		var playerStateEntitiesSet = contexts.PlayerState.GetEntitiesWithState(stateHolderId);
		if (playerStateEntitiesSet.Count > 1)
		{
			Debug.LogError($"There should only be at most 1 state targetting a state holder at the same time, but there are {playerStateEntitiesSet.Count}." +
				$"Something could be wrong!");
		}

		// We need to do copy the list because playerStateEntitiesSet is a shared HashSet that could be modified during foreach iteration, which is not allowed.
		// Thus we copy HashSet to a preallocated array to avoid heap/GC allocation, and do the destruction process with that array.
		playerStateEntitiesSet.CopyTo(s_PreallocatedPlayerStateEntitiesToBeDestroyed);
		for (int i = 0; i < playerStateEntitiesSet.Count; i++)
		{
			var playerStateEntity = s_PreallocatedPlayerStateEntitiesToBeDestroyed[i];
			contexts.DestroyPlayerStateEntity(playerStateEntity);
		}

		return;
	}

	public static void DestroyPlayerStateEntity(this Contexts contexts, PlayerStateEntity playerStateEntity)
	{
		int stateHolderId = playerStateEntity.State.HolderId;
		playerStateEntity.Destroy();

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsLeaveState = true;
	}

	public static TryAddPlayerStateTypeResult TryAddPlayerStateTypeFor(this Contexts contexts, StateTypeEnum stateType, int stateHolderId)
	{
		if (!contexts.Config.GameConfig.value.StateTypeFactory.TryGetStateBlueprint(stateType, out var blueprint))
		{
			return new TryAddPlayerStateTypeResult() { Success = false };
		}

		PlayerStateEntity newStateEntity = contexts.PlayerState.CreateEntity();
		blueprint.ApplyToEntity(newStateEntity);
		newStateEntity.AddState(stateHolderId);
		newStateEntity.AddStateFactoryType(stateType);

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsEnterState = true;

		return new TryAddPlayerStateTypeResult() { Success = true, PlayerStateEntity = newStateEntity };
	}

	public static PlayerStateEntity AddPlayerStateFor(this Contexts contexts, int stateHolderId)
	{
		PlayerStateEntity newStateEntity = contexts.PlayerState.CreateEntity();
		newStateEntity.AddState(stateHolderId);

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsEnterState = true;

		return newStateEntity;
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

	public static TryCommandSpawnItemResult TryCommandSpawnItemAt(this Contexts contexts, IItemBlueprint itemBlueprint, Vector2Int tilePosition)
	{
		ItemEntity itemEntity = contexts.Item.GetEntityWithOnTileItem(tilePosition);
		if (itemEntity != null)
		{
			// There is already an item on the given location. Command not sent.
			Debug.LogWarning($"There is already an item on the given position {tilePosition}.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		TileEntity tileEntity = contexts.Tile.GetEntityWithTilePosition(tilePosition);
		if (!tileEntity.IsItemHolder)
		{
			// The tile at the given position is not an item holder.
			Debug.LogWarning($"The tile at the given position {tilePosition} is not an item holder.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		if (!tileEntity.IsEnterable)
		{
			// Normally the system that creates the spawn command should check if the location is valid first!
			Debug.LogWarning($"The tile at the given position {tilePosition} is not enterable.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		// Create spawn item command entity.
		var command = contexts.Command.CreateEntity();
		command.AddSpawnItem(tilePosition, itemBlueprint);
		return new TryCommandSpawnItemResult { Success = true, CommandEntity = command };
	}

	public static ItemEntity CreateGlobalItemSpawner(this Contexts contexts, IItemBlueprint[] itemBlueprints, float spawnInterval, int maxItemCountOnLevel)
	{
		ItemEntity spawnerEntity = contexts.Item.CreateEntity();
		spawnerEntity.AddGlobalItemSpawner(contexts.Level.GetNewItemSpawnerId(), itemBlueprints);
		spawnerEntity.AddSpawnInterval(spawnInterval);
		spawnerEntity.AddMaxNumberOfItemsInLevel(maxItemCountOnLevel);
		return spawnerEntity;
	}

	public static ElementEntity ConstructPlayerEntity(this Contexts contexts, int playerId, int teamId, int skinId)
	{
		IPlayerFactory playerFactory = contexts.Config.GameConfig.value.PlayerFactory;

		// Create player entity and its view controller
		ElementEntity playerEntity = contexts.Element.CreateEntity();
		IEntityCreationEventController viewController = playerFactory.CreatePlayerView(playerId, teamId, skinId);
		viewController.OnEntityCreated(contexts, playerEntity);

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

		viewController.OnComponentsAdded(contexts, playerEntity);

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

	public static int GetNewItemSpawnerId(this LevelContext context)
	{
		int id = context.ItemSpawnerIdCounter.value.Value;
		context.ReplaceItemSpawnerIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static TileEntity ConstructTileEntityAtPosition(this Contexts contexts, TileData tileData, Vector2Int tilePosition)
	{
		ITileFactory tileFactory = contexts.Config.GameConfig.value.TileFactory;

		string tileId = tileData.TileId;

		// Create entity and its view controller
		var tileEntity = contexts.Tile.CreateEntity();
		IEntityCreationEventController viewController = tileFactory.CreateTileView(tileId);
		viewController.OnEntityCreated(contexts, tileEntity);

		// Apply blueprint and components
		var blueprint = tileFactory.GetTileBlueprint(tileId);
		blueprint.ApplyToEntity(tileEntity);
		tileEntity.AddTilePosition(tilePosition);
		viewController.OnComponentsAdded(contexts, tileEntity);

		// Link view controller with entity
		viewController.Link(tileEntity);

		// TODO: remove this!
		/*
		if (Random.Range(0.0f, 1.0f) > 0.8f)
		{
			var itemEntity = contexts.Item.CreateEntity();
			itemEntity.AddOnTileItem(tilePosition);
			itemEntity.AddApplySpeedChangeStateForEaterOnEaten(3.0f, 2.0f);
		}*/

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

	/// <summary>
	/// Use this instead of InputContext.DestoryAllEntities, because some of the input components hold unmanaged data (i.e. AIInput.SearchSimulationState)
	/// </summary>
	/// <param name="context"></param>
	public static void DeallocateAndDestroyAllEntities(this InputContext context)
	{
		// Deallocate AIInput simulation state data
		var aiInputGroup = context.GetGroup(InputMatcher.AIInput);
		foreach (var inputEntity in aiInputGroup)
		{
			if (inputEntity.HasEvaluatingForMovementInput)
			{
				inputEntity.EvaluatingForMovementInput.JobHandle.Complete();
				inputEntity.EvaluatingForMovementInput.Job.ResultContainer.Dispose();
			}

			inputEntity.AIInput.SearchSimulationState.Deallocate();
		}

		context.DestroyAllEntities();
	}
}