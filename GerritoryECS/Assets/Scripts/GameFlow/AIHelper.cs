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
	/// <see cref="GameSimulationState"/> stores important relevant game state used during AI search algorithm.
	/// </summary>
	public struct GameSimulationState
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


		public void InitializeWithContexts(Contexts contexts, Allocator allocator)
		{
			AllocatorType = allocator;

			// Allocate Tile related states
			int numberOfTiles = contexts.Config.GameConfig.value.LevelData.TileDataPairs.Count;
			Vector2Int levelBoundingRectSize = contexts.Config.GameConfig.value.LevelData.LevelBoundingRectSize;

			TileIndices = new NativeArray<int>(levelBoundingRectSize.x * levelBoundingRectSize.y, allocator);
			TileOwnerTeamIds = new NativeArray<int>(numberOfTiles, allocator);
			TileWorthPoints = new NativeArray<int>(numberOfTiles, allocator);
			TileItems = new NativeArray<bool>(numberOfTiles, allocator);

			var tilePositions = contexts.Config.GameConfig.value.LevelData.TileDataPairs.
				OrderBy(tileDataPair => tileDataPair.Key).
				Select(tileDataPair => tileDataPair.Key).
				ToArray();

			for (int i = 0; i < TileIndices.Length; i++)
			{
				TileIndices[i] = -1;
			}

			for (int i = 0; i < tilePositions.Length; i++)
			{
				var tilePosition = tilePositions[i];
				TileEntity tile = contexts.Tile.GetEntityWithTilePosition(tilePosition);
				int oneDimensionIndex = tilePositionTo1DArrayIndex(tilePosition, levelBoundingRectSize);
				TileIndices[oneDimensionIndex] = i;
				TileOwnerTeamIds[i] = tile.HasOwnable && tile.HasOwner? tile.Owner.OwnerTeamId : -1;
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

			for (int i = 0; i < numberOfRelevantOnTileElements; i++)
			{
				ElementEntity element = relevantElements[i];
				OnTileElementIds[i] = element.OnTileElement.Id;
				OnTileElementTeamIds[i] = element.Team.Id;
				OnTileElementPositions[i] = element.OnTilePosition.Value;
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
	public static int GetIndexOfTileAt(this GameSimulationState gameSimulationState, Vector2Int targetPosition)
	{
		int tileArrayIndex = targetPosition.x * gameSimulationState.LevelBoundingRectSize.y + targetPosition.y;
		return gameSimulationState.TileIndices[tileArrayIndex];
	}

	/// <summary>
	/// Try to get the index of an OnTileElement with the given id.
	/// </summary>
	/// <param name="gameSimulationState"></param>
	/// <param name="position"></param>
	/// <returns>The index of the OnTileElement; otherwise return -1 if the OnTileElement with the id is not in the list.</returns>
	public static int GetIndexOfOnTileElementWithId(this GameSimulationState gameSimulationState, int targetOnTileElementId)
	{
		for (int i = 0; i < gameSimulationState.OnTileElementIds.Length; i++)
		{
			var onTileElementId = gameSimulationState.OnTileElementIds[i];
			if (onTileElementId == targetOnTileElementId)
			{
				return i;
			}
		}

		return -1;
	}

	public static float EvaluateScoreEarnedIfOnTileElementMoveTo(this GameSimulationState gameSimulationState, int onTileElementId, Vector2Int toPosition, int temporalRelevancy)
	{
		float scoreEarned = 0;
		int mappedTileIndex = gameSimulationState.GetIndexOfTileAt(toPosition);
		int mappedOnTileElementIndex = gameSimulationState.GetIndexOfOnTileElementWithId(onTileElementId);
		int teamId = gameSimulationState.OnTileElementTeamIds[mappedOnTileElementIndex];

		// Evaluate the tile based on its ownership.
		bool tileIsOwnable = gameSimulationState.TileWorthPoints[mappedTileIndex] > 0;
		if (tileIsOwnable)
		{
			int worthPoints = gameSimulationState.TileWorthPoints[mappedTileIndex];

			int ownerTeamId = gameSimulationState.TileOwnerTeamIds[mappedTileIndex];
			bool tileHasOwner = ownerTeamId != -1;
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

		// TODO: Evaluate potential prey/predator, need more data to achieve this. (i.e. Priority)
		// ...

		return scoreEarned;
	}

	/// <summary>
	/// Update game simulation state as if the given OnTileElement move to the position.
	/// </summary>
	/// <param name="gameSimulationState"></param>
	/// <param name="onTileElementId"></param>
	/// <param name="toPosition"></param>
	public static void MoveOnTileElementTo(this GameSimulationState gameSimulationState, int onTileElementId, Vector2Int toPosition)
	{

	}
}
