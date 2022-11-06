using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.InputManagerEntry;

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
	public CommandEntity CommandEntity;
}

public struct TryMakeGhostDisppearResult
{
	public enum ResultType
	{
		Success,
		IsAlreadyDisappearing,
		CannotBeForceKilled
	}

	public ResultType Type;
}

public struct TryMakeGhostReappearResult
{
	public enum ResultType
	{
		Success,
		IsAlreadyAppearing,
		HasNoValidReappearPosition
	}

	public ResultType Type;
}

public static partial class GameHelper
{
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

	private static IGroup<TileEntity> s_GhostSpawnTileGroup = null;
	public static TryGetValidRespawnPositionResult TryGetValidGhostSpawnPosition(this Contexts contexts, ElementEntity ghostEntity, int spawnAreaId)
	{
		if (s_GhostSpawnTileGroup == null)
		{
			// The ghost cannot spawn on respawn tile!
			s_GhostSpawnTileGroup =
				contexts.Tile.GetGroup(TileMatcher.AllOf(TileMatcher.TilePosition, TileMatcher.Enterable).NoneOf(TileMatcher.CanBeRespawnedOn));
		}

		var validTileEntities = s_GhostSpawnTileGroup.GetEntities().Where(
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
			Debug.LogError($"Cannot find valid tiles for the Ghost entity {ghostEntity.OnTileElement.Id} to be spawned on.");
			return new TryGetValidRespawnPositionResult() { Success = false, TilePosition = Vector2Int.zero };
		}

		TileEntity randomlyPickedTileEntity = validTileEntities.ElementAt(UnityEngine.Random.Range(0, validTileEntities.Count()));
		return new TryGetValidRespawnPositionResult() { Success = true, TilePosition = randomlyPickedTileEntity.TilePosition.Value };
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

		var existingKillCommands = contexts.Command.GetEntitiesWithMarkOnTileElementDead(onTileEntity.OnTileElement.Id);
		if (existingKillCommands.Count > 0)
		{
			Debug.LogWarning($"Another kill command has already been issued against the entity {onTileEntity}. This one will be ignored!");
			return new TryCommandKillResult { Success = false };
		}

		// Create mark dead request entity.
		var commandEntity = contexts.Command.CreateEntity();
		commandEntity.AddMarkOnTileElementDead(onTileEntity.OnTileElement.Id);
		return new TryCommandKillResult { Success = true, CommandEntity = commandEntity };
	}

	/// <summary>
	/// This function unlike <see cref="TryCommandKill(Contexts, ElementEntity)"/>, it doesn't check whether the entity has CanBeDead component or not.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="onTileEntity"></param>
	/// <returns></returns>
	public static TryCommandKillResult TryCommandForceKillImmortal(this Contexts contexts, ElementEntity onTileEntity)
	{
		if (onTileEntity.IsDead)
		{
			Debug.LogWarning("The entity is already dead. Cannot be killed again.");
			return new TryCommandKillResult { Success = false };
		}

		var existingKillCommands = contexts.Command.GetEntitiesWithMarkOnTileElementDead(onTileEntity.OnTileElement.Id);
		if (existingKillCommands.Count > 0)
		{
			Debug.LogWarning($"Another kill command has already been issued against the entity {onTileEntity}. This one will be ignored!");
			return new TryCommandKillResult { Success = false };
		}

		// Create mark dead request entity.
		var commandEntity = contexts.Command.CreateEntity();
		commandEntity.AddMarkOnTileElementDead(onTileEntity.OnTileElement.Id);
		return new TryCommandKillResult { Success = true, CommandEntity = commandEntity };
	}

	/// <summary>
	/// Remove the given OnTileEntity from the level, namely remove its OnTilePosition and possible MoveOnTile components.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="onTileEntity"></param>
	/// <returns></returns>
	public static void RemoveOnTileElementPositionFromLevel(this Contexts contexts, ElementEntity onTileEntity)
	{
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
	}

	public static TryMakeGhostDisppearResult TryMakeGhostDisappear(this Contexts contexts, ElementEntity elementEntity)
	{
		if (elementEntity.HasGhostDisappearing)
		{
			return new TryMakeGhostDisppearResult { Type = TryMakeGhostDisppearResult.ResultType.IsAlreadyDisappearing };
		}

		float initialDisappearingProgress = 0;
		if (elementEntity.HasGhostAppearing)
		{
			// If the ghost is in the process of appearing, invert that progress as the disappearing progress.
			initialDisappearingProgress = 1.0f - elementEntity.GhostAppearing.Progress;
			elementEntity.RemoveGhostAppearing();
		}

		// Add disappearing counter component.
		elementEntity.AddGhostDisappearing(initialDisappearingProgress);

		// Kill the ghost.
		TryCommandKillResult killResult = contexts.TryCommandForceKillImmortal(elementEntity);
		if (!killResult.Success)
		{
			// The kill action is not successful.
			return new TryMakeGhostDisppearResult { Type = TryMakeGhostDisppearResult.ResultType.CannotBeForceKilled };
		}

		return new TryMakeGhostDisppearResult { Type = TryMakeGhostDisppearResult.ResultType.Success };
	}

	public static TryMakeGhostReappearResult TryMakeGhostReappear(this Contexts contexts, ElementEntity elementEntity)
	{
		if (elementEntity.HasGhostAppearing)
		{
			Debug.LogWarning("The ghost entity is already in the process of appearing!");
			return new TryMakeGhostReappearResult { Type = TryMakeGhostReappearResult.ResultType.IsAlreadyAppearing };
		}

		float initialAppearingProgress = 0;
		if (elementEntity.HasGhostDisappearing)
		{
			// If the ghost is in the process of disappearing, invert that progress as the appearing progress.
			initialAppearingProgress = 1.0f - elementEntity.GhostDisappearing.Progress;
			elementEntity.RemoveGhostDisappearing();
		}

		// Get a position to revive the ghost on.
		TryGetValidRespawnPositionResult result = contexts.TryGetValidGhostSpawnPosition(elementEntity, 0);
		if (result.Success)
		{
			// Add appearing component to revive the ghost later.
			elementEntity.AddGhostAppearing(initialAppearingProgress);

			// Add new position.
			elementEntity.ReplaceOnTilePosition(result.TilePosition);

			return new TryMakeGhostReappearResult { Type = TryMakeGhostReappearResult.ResultType.Success };
		}
		else
		{
			Debug.LogError("Cannot find a valid position to spawn the ghost! Not reappeared.");
			return new TryMakeGhostReappearResult { Type = TryMakeGhostReappearResult.ResultType.HasNoValidReappearPosition };
		}
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

	public const int k_FatalThreatPriority = -1;
	private const int k_InivinciblePriority = k_FatalThreatPriority;
	private const int k_GhsotPriority = k_FatalThreatPriority - 1;
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

		// If it's a ghost, the priority is k_GhostPriority
		if (onTileEntity.IsGhost)
		{
			return k_GhsotPriority;
		}

		return priority;
	}

	public static ElementEntity ConstructPlayerEntity(this Contexts contexts, int playerId, int teamId, int skinId)
	{
		IOnTileElementFactory elementFactory = contexts.Config.GameConfig.value.OnTileElementFactory;

		// Create player entity and its view controller
		ElementEntity playerEntity = contexts.Element.CreateEntity();
		IEntityCreationEventController viewController = elementFactory.CreatePlayerView(playerId, teamId, skinId);
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
		playerEntity.IsOnTileElementStepKiller = true;

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

	/// <summary>
	/// Note that this function only creates the ghost entity but not the input entity that would control the ghost.
	/// Use <see cref="ConstructChaseNearestOnTileElementInputEntity(ElementEntity, Contexts)"/> to create an input entity that would control the ghost to chase other targets.
	/// </summary>
	/// <param name="contexts"></param>
	/// <returns></returns>
	public static ElementEntity ConstructGhostEntity(this Contexts contexts)
	{
		IOnTileElementFactory elementFactory = contexts.Config.GameConfig.value.OnTileElementFactory;

		// Create player entity and its view controller
		ElementEntity ghostEntity = contexts.Element.CreateEntity();
		IEntityCreationEventController viewController = elementFactory.CreateGhostView();
		viewController.OnEntityCreated(contexts, ghostEntity);

		// Add needed componenets
		ghostEntity.IsGhost = true;
		ghostEntity.IsOnTileElementStepKiller = true;
		ghostEntity.IsCanDisappearAfterStepKill = true;
		ghostEntity.AddOnTileElement(contexts.Level.GetNewOnTileElementId());
		ghostEntity.AddSpeedChangeable(GameConstants.GhostBaseSpeed, 1);

		TryGetValidRespawnPositionResult result = contexts.TryGetValidGhostSpawnPosition(ghostEntity, 0);
		if (result.Success)
		{
			ghostEntity.AddOnTilePosition(result.TilePosition);
		}
		else
		{
			Debug.LogError("Cannot find a valid position to spawn the ghost, place the ghost at (0, 0)");
			ghostEntity.AddOnTilePosition(Vector2Int.zero);
		}

		contexts.Message.EmitOnTileElementEnterTileMessage(ghostEntity.OnTileElement.Id, ghostEntity.OnTilePosition.Value);

		viewController.OnComponentsAdded(contexts, ghostEntity);

		// Link view controller with entity
		viewController.Link(ghostEntity);

		return ghostEntity;
	}

	public static InputEntity ConstructChaseNearestOnTileElementInputEntity(this ElementEntity entity, Contexts contexts)
	{
		var inputEntity = contexts.Input.CreateEntity();
		inputEntity.AddChaseNearestOnTileElementVictimInput(entity.OnTileElement.Id, GameConstants.MaxGhostChaseVictimHeuristicDistance, new AIHelper.PathfindingSimulationState());
		inputEntity.ChaseNearestOnTileElementVictimInput.PathfindingSimulationState.AllocateWithContexts(contexts, Unity.Collections.Allocator.Persistent);
		return inputEntity;
	}

	public static bool TryMakeDisappear(this ElementEntity elementEntity)
	{
		if (!elementEntity.IsGhost)
		{
			Debug.LogError("You are not supposed to make a non-ghost element entity disappear.");
			return false;
		}

		if (elementEntity.HasGhostDisappearing)
		{
			Debug.LogError($"The entity {elementEntity.OnTileElement.Id} is already disappearing.");
			return false;
		}

		elementEntity.AddGhostDisappearing(0);
		return true;
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
