using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using static AIHelper;

public static class AIHelper
{
	/// <summary>
	/// <see cref="SearchSimulationState"/> stores important relevant game state used during AI search algorithm.
	/// </summary>
	public struct SearchSimulationState
	{
		public bool IsInitialized { get; private set; }

		public Allocator AllocatorType;

		public Vector2Int LevelBoundingRectSize;

		/// <summary>
		/// An array of indices for tiles that should be used to access other arrays of data in the struct.
		/// An element with the value of -1 means there is no tile at that position.
		/// </summary>
		public NativeArray<int> TileIndices;
		/// <summary>
		/// Indexed with <see cref="TileIndices"/>, indicating which tiles are owned. An element with the value of -1 means it is not owned by any team yet.
		/// </summary>
		public NativeArray<int> TileOwnerTeamIds;
		/// <summary>
		/// Indexed with <see cref="TileIndices"/>, indicating how many points tiles worth. A value of 0 means it is currently not occupyable.
		/// </summary>
		public NativeArray<int> TileWorthPoints;
		/// <summary>
		/// Indexed with <see cref="TileIndices"/>, indicating if tiles have items/powerups on them.
		/// </summary>
		public NativeArray<bool> TileItems;

		/// <summary>
		/// An array of ids of the OnTileElements in the game.
		/// </summary>
		public NativeArray<int> OnTileElementIds;
		/// <summary>
		/// Indexed with <see cref="OnTileElementIds"/>, the team ids of OnTileElements.
		/// </summary>
		public NativeArray<int> OnTileElementTeamIds;
		/// <summary>
		/// Indexed with <see cref="OnTileElementIds"/>, the positions of OnTileElements.
		/// </summary>
		public NativeArray<Vector2Int> OnTileElementPositions;
		/// <summary>
		/// Indexed with <see cref="OnTileElementIds"/>, the priority of OnTileElements. We don't update this regularly.
		/// </summary>
		public NativeArray<int> OnTileElementPriorities;

		public const int k_NotOwnableTeamId = -2;
		public const int k_NoOwnerTeamId = -1;
		public const int k_NoTile = -1;
		public const int k_NoElement = -1;


		public void InitializeWithContexts(Contexts contexts, Allocator allocator)
		{
			AllocatorType = allocator;

			// Allocate Tile related states
			int numberOfTiles = contexts.Config.GameConfig.value.LevelData.TileDataPairs.Count;
			LevelBoundingRectSize = contexts.Config.GameConfig.value.LevelData.LevelBoundingRectSize;

			TileIndices = new NativeArray<int>(LevelBoundingRectSize.x * LevelBoundingRectSize.y, allocator);
			TileOwnerTeamIds = new NativeArray<int>(numberOfTiles, allocator);
			TileWorthPoints = new NativeArray<int>(numberOfTiles, allocator);
			TileItems = new NativeArray<bool>(numberOfTiles, allocator);

			var tilePositions = contexts.Config.GameConfig.value.LevelData.TileDataPairs.
				OrderBy(tileDataPair => tileDataPair.Key.x).
				ThenBy(tileDataPair => tileDataPair.Key.y).
				Select(tileDataPair => tileDataPair.Key).
				ToArray();

			for (int i = 0; i < TileIndices.Length; i++)
			{
				TileIndices[i] = k_NoTile;
			}

			for (int i = 0; i < tilePositions.Length; i++)
			{
				var tilePosition = tilePositions[i];
				TileEntity tile = contexts.Tile.GetEntityWithTilePosition(tilePosition);
				int oneDimensionIndex = tilePositionTo1DArrayIndex(tilePosition, LevelBoundingRectSize);
				TileIndices[oneDimensionIndex] = i;
				
				if (!tile.HasOwnable)
				{
					TileOwnerTeamIds[i] = k_NotOwnableTeamId;
				}
				else
				{
					TileOwnerTeamIds[i] = tile.HasOwner ? tile.Owner.OwnerTeamId : k_NoOwnerTeamId;
				}

				TileWorthPoints[i] = tile.HasOwnable ? tile.Ownable.WorthPoints : 0;
				TileItems[i] = tile.IsItemHolder && contexts.Item.GetEntityWithOnTileItem(tilePosition) != null;
			}


			// Allocate OnTileElement related states
			ElementEntity[] relevantElements = contexts.Element.GetGroup
			(
				ElementMatcher.AllOf
				(
					ElementMatcher.OnTileElement, 
					ElementMatcher.OnTilePosition,
					ElementMatcher.TileOwner, 
					ElementMatcher.Team
				)
			).GetEntities();
			int numberOfRelevantOnTileElements = relevantElements.Length;

			OnTileElementIds = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementTeamIds = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementPositions = new NativeArray<Vector2Int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementPriorities = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);

			for (int i = 0; i < numberOfRelevantOnTileElements; i++)
			{
				ElementEntity element = relevantElements[i];
				OnTileElementIds[i] = element.OnTileElement.Id;
				OnTileElementTeamIds[i] = element.Team.Id;
				OnTileElementPositions[i] = element.OnTilePosition.Value;
				OnTileElementPriorities[i] = contexts.GetOnTileElementKillPriority(element);
			}

			IsInitialized = true;
		}

		public void Deallocate()
		{
			if (!IsInitialized)
			{
				Debug.LogError("The game simulation state is not initialized, no need to deallocate the data.");
				return;
			}

			TileIndices.Dispose();
			TileOwnerTeamIds.Dispose();
			TileItems.Dispose();

			OnTileElementIds.Dispose();
			OnTileElementTeamIds.Dispose();
			OnTileElementPositions.Dispose();

			IsInitialized = false;
		}

		private int tilePositionTo1DArrayIndex(Vector2Int position, Vector2Int levelSize)
		{
			int tileArrayIndex = position.x * levelSize.y + position.y;
			return tileArrayIndex;
		}
	}

	/// <summary>
	/// Try to get the index of a tile on the given position.
	/// </summary>
	/// <param name="gameSimulationState"></param>
	/// <param name="targetPosition"></param>
	/// <returns>The index of the tile position; otherwise return -1 if the position doesn't have a tile.</returns>
	public static int GetIndexOfTileAt(this SearchSimulationState gameSimulationState, Vector2Int targetPosition)
	{
		bool outOfBounds = targetPosition.x < 0 || targetPosition.x >= gameSimulationState.LevelBoundingRectSize.x ||
						   targetPosition.y < 0 || targetPosition.y >= gameSimulationState.LevelBoundingRectSize.y;

		if (outOfBounds)
		{
			return SearchSimulationState.k_NoTile;
		}

		int tileArrayIndex = targetPosition.x * gameSimulationState.LevelBoundingRectSize.y + targetPosition.y;
		return gameSimulationState.TileIndices[tileArrayIndex];
	}

	/// <summary>
	/// Try to get the index of an OnTileElement with the given id.
	/// </summary>
	/// <param name="gameSimulationState"></param>
	/// <param name="position"></param>
	/// <returns>The index of the OnTileElement; otherwise return -1 if the OnTileElement with the id is not in the list.</returns>
	public static int GetIndexOfOnTileElementWithId(this SearchSimulationState gameSimulationState, int targetOnTileElementId)
	{
		for (int i = 0; i < gameSimulationState.OnTileElementIds.Length; i++)
		{
			var onTileElementId = gameSimulationState.OnTileElementIds[i];
			if (onTileElementId == targetOnTileElementId)
			{
				return i;
			}
		}

		return SearchSimulationState.k_NoElement;
	}

	public static float EvaluateScoreEarnedIfOnTileElementMoveTo(this SearchSimulationState gameSimulationState, int onTileElementId, Vector2Int toPosition, int temporalRelevancy)
	{
		float scoreEarned = 0;
		int mappedTileIndex = gameSimulationState.GetIndexOfTileAt(toPosition);
		if (mappedTileIndex == SearchSimulationState.k_NoTile)
		{
			// If the given position doesn't have a tile, the score earned is always zero.
			return 0;
		}

		int mappedOnTileElementIndex = gameSimulationState.GetIndexOfOnTileElementWithId(onTileElementId);
		int teamId = gameSimulationState.OnTileElementTeamIds[mappedOnTileElementIndex];

		// Evaluate the tile based on its ownership.
		bool tileIsOwnable = gameSimulationState.TileWorthPoints[mappedTileIndex] > 0;
		if (tileIsOwnable)
		{
			int worthPoints = gameSimulationState.TileWorthPoints[mappedTileIndex];

			int ownerTeamId = gameSimulationState.TileOwnerTeamIds[mappedTileIndex];
			bool tileHasOwner = ownerTeamId != SearchSimulationState.k_NoOwnerTeamId && ownerTeamId != SearchSimulationState.k_NotOwnableTeamId;
			if (!tileHasOwner)
			{
				// If the tile doesn't have an owner, move to it will reward the agent with points.
				scoreEarned += (1 + temporalRelevancy * 0.1f) * worthPoints;
			}

			bool isOwnedByDifferentTeam = tileHasOwner && ownerTeamId != teamId;
			if (isOwnedByDifferentTeam)
			{
				// If the tile is owned by a different team, move to it will greatly reward the agent (increase own score, decrease opponent's score).
				scoreEarned += (1 + temporalRelevancy * 0.2f) * worthPoints;
			}
		}

		// Evaluate the tile based on the item/powerup
		if (gameSimulationState.TileItems[mappedTileIndex])
		{
			scoreEarned += (1 + temporalRelevancy * 0.3f);
		}

		// Evaluate if the occupying element is potential prey/predator
		for (int i = 0; i < gameSimulationState.OnTileElementPositions.Length; i++)
		{
			if (i == mappedOnTileElementIndex)
			{
				// Oneself cannot be a prey or predator, skip it.
				continue;
			}

			var onTileElementPosition = gameSimulationState.OnTileElementPositions[i];
			if (onTileElementPosition != toPosition)
			{
				// The OnTileElement is not at the location we are interested in, skip it.
				continue;
			}

			int occupierTeamId = gameSimulationState.OnTileElementTeamIds[i];
			bool isInTheSameTeam = occupierTeamId == teamId;
			if (isInTheSameTeam)
			{
				// The occupier is in the same team as the agent, therefore it is impossible to be a prey or a predator.
				break;
			}

			int opponentPriority = gameSimulationState.OnTileElementPriorities[i];
			int agentPriority = gameSimulationState.OnTileElementPriorities[mappedOnTileElementIndex];

			if (opponentPriority > agentPriority)
			{
				// The agent can kill this opponent, moving to this position rewards with a kill!
				// We square the temporalRelevancy because we only want to chase the target that is close enough, distant target is more unpredictable.
				scoreEarned += (1 + temporalRelevancy * temporalRelevancy * 0.5f);
			}
			else
			{
				// The opponent is possibly dangerous to the agent, moving away from this position to avoid death!
				scoreEarned -= (1 + temporalRelevancy * temporalRelevancy * 0.5f);
			}
		}

		return scoreEarned;
	}

	/// <summary>
	/// Update game simulation state as if the given OnTileElement move to the position.
	/// </summary>
	/// <param name="gameSimulationState"></param>
	/// <param name="onTileElementId"></param>
	/// <param name="toPosition"></param>
	public static void MoveOnTileElementTo(this SearchSimulationState gameSimulationState, int onTileElementId, Vector2Int toPosition)
	{
		// TODO: update the game state as if the on tile element moved to the given location
		int mappedOnTileElementIndex = gameSimulationState.GetIndexOfOnTileElementWithId(onTileElementId);
		int mappedTileIndex = gameSimulationState.GetIndexOfTileAt(toPosition);
		int teamId = gameSimulationState.OnTileElementTeamIds[mappedOnTileElementIndex];
		gameSimulationState.OnTileElementPositions[mappedOnTileElementIndex] = toPosition;

		if (gameSimulationState.TileOwnerTeamIds[mappedTileIndex] != SearchSimulationState.k_NotOwnableTeamId)
		{
			gameSimulationState.TileOwnerTeamIds[mappedTileIndex] = teamId;
		}
	}
}
