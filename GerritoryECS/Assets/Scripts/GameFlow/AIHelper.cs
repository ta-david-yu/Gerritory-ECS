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

public static partial class AIHelper
{
	/// <summary>
	/// <see cref="SearchSimulationState"/> stores important relevant game state used during AI search algorithm.
	/// </summary>
	public struct SearchSimulationState
	{
		public bool IsAllocated { get; private set; }

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
		/// <summary>
		/// Indexed with <see cref="OnTileElementIds"/>, indicating if OnTilElements are dead. We don't update his regularly
		/// </summary>
		public NativeArray<bool> AreOnTileElementsDead;

		public const int k_NotOwnableTeamId = -2;
		public const int k_NoOwnerTeamId = -1;
		public const int k_NoTile = -1;
		public const int k_NoElement = -1;
		public const int k_NoTeam = -1;
		public const float k_NeverDoActionScore = -100;

		public SearchSimulationState AllocateWithContexts(Contexts contexts, Allocator allocator)
		{
			AllocatorType = allocator;

			// Allocate Tile related states
			int numberOfTiles = contexts.Config.GameConfig.value.LevelData.TileDataPairs.Count;
			LevelBoundingRectSize = contexts.Config.GameConfig.value.LevelData.LevelBoundingRectSize;
			TileIndices = new NativeArray<int>(LevelBoundingRectSize.x * LevelBoundingRectSize.y, allocator);
			TileOwnerTeamIds = new NativeArray<int>(numberOfTiles, allocator);
			TileWorthPoints = new NativeArray<int>(numberOfTiles, allocator);
			TileItems = new NativeArray<bool>(numberOfTiles, allocator);

			// Allocate OnTileElement related states
			ElementEntity[] relevantElements = contexts.Element.GetGroup
			(
				ElementMatcher.AllOf
				(
					ElementMatcher.OnTileElement,
					ElementMatcher.TileOwner,
					ElementMatcher.Team
				)
			).GetEntities();
			int numberOfRelevantOnTileElements = relevantElements.Length;
			OnTileElementIds = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementTeamIds = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementPositions = new NativeArray<Vector2Int>(numberOfRelevantOnTileElements, allocator);
			OnTileElementPriorities = new NativeArray<int>(numberOfRelevantOnTileElements, allocator);
			AreOnTileElementsDead = new NativeArray<bool>(numberOfRelevantOnTileElements, allocator);

			IsAllocated = true;

			return this;
		}

		public void InitializeWithContexts(Contexts contexts)
		{
			if (!IsAllocated)
			{
				Debug.LogError($"The SearchSimulationState is not allocated yet, cannot initialize data!");
				return;
			}

			// Initialize tile related data
			int numberOfTiles = contexts.Config.GameConfig.value.LevelData.TileDataPairs.Count;
			LevelBoundingRectSize = contexts.Config.GameConfig.value.LevelData.LevelBoundingRectSize;

			Vector2Int[] tilePositions;
			if (contexts.Level.HasSearchSimulationGlobalState)
			{
				tilePositions = contexts.Level.SearchSimulationGlobalState.SortedTilePositions;
			}
			else
			{
				tilePositions = contexts.Config.GameConfig.value.LevelData.TileDataPairs.
								OrderBy(tileDataPair => tileDataPair.Key.x).
								ThenBy(tileDataPair => tileDataPair.Key.y).
								Select(tileDataPair => tileDataPair.Key).
								ToArray();
				contexts.Level.SetSearchSimulationGlobalState(tilePositions);
			}

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
					ElementMatcher.TileOwner, 
					ElementMatcher.Team
				)
			).GetEntities();
			int numberOfRelevantOnTileElements = relevantElements.Length;

			for (int i = 0; i < numberOfRelevantOnTileElements; i++)
			{
				ElementEntity element = relevantElements[i];
				OnTileElementIds[i] = element.OnTileElement.Id;
				OnTileElementTeamIds[i] = element.Team.Id;

				float aiNoticeMovementDelay = 0.7f;
				float aiNoticeMovementStagger = 0.25f;
				aiNoticeMovementDelay += Random.Range(-aiNoticeMovementStagger, aiNoticeMovementStagger);
				if (element.HasMoveOnTile && element.MoveOnTile.Progress > aiNoticeMovementDelay)
				{
					// If the element is moving, take its destination as the position.
					OnTileElementPositions[i] = element.MoveOnTile.ToPosition;
				}
				else if (element.HasOnTilePosition)
				{
					OnTileElementPositions[i] = element.OnTilePosition.Value;
				}
				else
				{
					OnTileElementPositions[i] = Vector2Int.one * -1;
				}

				AreOnTileElementsDead[i] = element.IsDead;
				OnTileElementPriorities[i] = contexts.GetOnTileElementKillPriority(element);
			}
		}

		public void Deallocate()
		{
			if (!IsAllocated)
			{
				Debug.LogError("The game simulation state is not initialized, no need to deallocate the data.");
				return;
			}

			TileIndices.Dispose();
			TileOwnerTeamIds.Dispose();
			TileWorthPoints.Dispose();
			TileItems.Dispose();

			OnTileElementIds.Dispose();
			OnTileElementTeamIds.Dispose();
			OnTileElementPositions.Dispose();
			OnTileElementPriorities.Dispose();
			AreOnTileElementsDead.Dispose();

			IsAllocated = false;
		}

		private int tilePositionTo1DArrayIndex(Vector2Int position, Vector2Int levelSize)
		{
			int tileArrayIndex = position.x * levelSize.y + position.y;
			return tileArrayIndex;
		}


		public struct OnTileElementAction
		{
			public int OnTileElementId;
			public Vector2Int MoveToPosition;

			private bool hasBeenApplied;
			private Vector2Int m_PreviousOnTileElementPosition;
			private int m_PreviousOwnerTeamIdAtMoveToPosition;

			public void Apply(ref SearchSimulationState searchSimulationState)
			{
				int mappedOnTileElementIndex = searchSimulationState.GetIndexOfOnTileElementWithId(OnTileElementId);
				int mappedTileIndex = searchSimulationState.GetIndexOfTileAt(MoveToPosition);

				m_PreviousOnTileElementPosition = searchSimulationState.OnTileElementPositions[mappedOnTileElementIndex];
				m_PreviousOwnerTeamIdAtMoveToPosition = searchSimulationState.TileOwnerTeamIds[mappedTileIndex];

				searchSimulationState.OnTileElementPositions[mappedOnTileElementIndex] = MoveToPosition;
				if (searchSimulationState.TileOwnerTeamIds[mappedTileIndex] != SearchSimulationState.k_NotOwnableTeamId)
				{
					int teamId = searchSimulationState.OnTileElementTeamIds[mappedOnTileElementIndex];
					searchSimulationState.TileOwnerTeamIds[mappedTileIndex] = teamId;
				}

				hasBeenApplied = true;
			}

			public void Revert(ref SearchSimulationState searchSimulationState)
			{
				if (!hasBeenApplied)
				{
					// The action has not been applied, revert will result in faulty state.
					return;
				}

				int mappedOnTileElementIndex = searchSimulationState.GetIndexOfOnTileElementWithId(OnTileElementId);
				int mappedTileIndex = searchSimulationState.GetIndexOfTileAt(MoveToPosition);

				searchSimulationState.OnTileElementPositions[mappedOnTileElementIndex] = m_PreviousOnTileElementPosition;
				searchSimulationState.TileOwnerTeamIds[mappedTileIndex] = m_PreviousOwnerTeamIdAtMoveToPosition;
			}
		}
	}

	/// <summary>
	/// Try to get the index of a tile on the given position.
	/// </summary>
	/// <param name="searchSimulationState"></param>
	/// <param name="targetPosition"></param>
	/// <returns>The index of the tile position; otherwise return -1 (k_NoTile) if the position doesn't have a tile.</returns>
	public static int GetIndexOfTileAt(this SearchSimulationState searchSimulationState, Vector2Int targetPosition)
	{
		bool outOfBounds = targetPosition.x < 0 || targetPosition.x >= searchSimulationState.LevelBoundingRectSize.x ||
						   targetPosition.y < 0 || targetPosition.y >= searchSimulationState.LevelBoundingRectSize.y;

		if (outOfBounds)
		{
			return SearchSimulationState.k_NoTile;
		}

		int tileArrayIndex = targetPosition.x * searchSimulationState.LevelBoundingRectSize.y + targetPosition.y;
		return searchSimulationState.TileIndices[tileArrayIndex];
	}

	/// <summary>
	/// Try to get the index of an OnTileElement with the given id.
	/// </summary>
	/// <param name="searchSimulationState"></param>
	/// <param name="position"></param>
	/// <returns>The index of the OnTileElement; otherwise return -1 if the OnTileElement with the id is not in the list.</returns>
	public static int GetIndexOfOnTileElementWithId(this SearchSimulationState searchSimulationState, int targetOnTileElementId)
	{
		for (int i = 0; i < searchSimulationState.OnTileElementIds.Length; i++)
		{
			var onTileElementId = searchSimulationState.OnTileElementIds[i];
			if (onTileElementId == targetOnTileElementId)
			{
				return i;
			}
		}

		return SearchSimulationState.k_NoElement;
	}

	public struct TryGetPositionResult
	{
		public bool Success;
		public Vector2Int Position;
	}

	public static TryGetPositionResult TryGetPositionOfOnTileElementWithId(this SearchSimulationState searchSimulationState, int targetOnTileElementId)
	{
		int index = GetIndexOfOnTileElementWithId(searchSimulationState, targetOnTileElementId);

		if (index == SearchSimulationState.k_NoElement)
		{
			return new TryGetPositionResult() { Success = false };
		}

		var position = searchSimulationState.OnTileElementPositions[index];
		return new TryGetPositionResult() { Success = true, Position = position };
	}

	public struct EvaluationParameters
	{
		/// <summary>
		/// How likely the agent would take over a tile. 0 means not influenced at all.
		/// </summary>
		public float TileOwnershipAffinity;

		/// <summary>
		/// How likely the agent would eat an item. 0 means not influenced at all.
		/// </summary>
		public float ItemAffinity;

		/// <summary>
		/// How likely the agent would try to attack predator. 0 means not influenced at all.
		/// </summary>
		public float Aggressiveness;

		/// <summary>
		/// How likely the agent would avoid powerful enemy. 0 means not influenced at all.
		/// </summary>
		public float Cautiousness;

		public static EvaluationParameters GetBasicBehaviourParameters()
		{
			return new EvaluationParameters()
			{
				TileOwnershipAffinity = 1,
				ItemAffinity = 1,
				Aggressiveness = 1,
				Cautiousness = 1,
			};
		}

		public static EvaluationParameters GetKillerBehaviourParameters()
		{
			return new EvaluationParameters()
			{
				TileOwnershipAffinity = 0,
				ItemAffinity = 1,
				Aggressiveness = 2,
				Cautiousness = 1,
			};
		}

		public static EvaluationParameters GetGreedyBehaviourParameters()
		{
			return new EvaluationParameters()
			{
				TileOwnershipAffinity = 2,
				ItemAffinity = 1,
				Aggressiveness = 0,
				Cautiousness = 1,
			};
		}

		public static EvaluationParameters GetPeacefulBehaviourParameters()
		{
			return new EvaluationParameters()
			{
				TileOwnershipAffinity = 1,
				ItemAffinity = 1,
				Aggressiveness = 0.2f,
				Cautiousness = 0.2f,
			};
		}
	}
	public static float EvaluateScoreEarnedIfOnTileElementMoveTo(this SearchSimulationState searchSimulationState, int onTileElementId, Vector2Int toPosition, EvaluationParameters evaluationParams, int temporalRelevancy)
	{
		float scoreEarned = 0;
		int mappedTileIndex = searchSimulationState.GetIndexOfTileAt(toPosition);
		if (mappedTileIndex == SearchSimulationState.k_NoTile)
		{
			// If the given position doesn't have a tile, cannot do the action.
			return SearchSimulationState.k_NeverDoActionScore;
		}

		int mappedOnTileElementIndex = searchSimulationState.GetIndexOfOnTileElementWithId(onTileElementId);
		int teamId = searchSimulationState.OnTileElementTeamIds[mappedOnTileElementIndex];

		Vector2Int originalPosition = searchSimulationState.OnTileElementPositions[mappedOnTileElementIndex];
		if (originalPosition == toPosition)
		{
			// Penalty for staying at the same location. 
			// The value needs to be bigger than the value of taking over a tile (1 + t * 0.1f).
			scoreEarned -= 1.2f;
		}

		// Evaluate the tile based on its ownership.
		bool tileIsOwnable = searchSimulationState.TileWorthPoints[mappedTileIndex] > 0;
		if (tileIsOwnable)
		{
			int worthPoints = searchSimulationState.TileWorthPoints[mappedTileIndex];

			int ownerTeamId = searchSimulationState.TileOwnerTeamIds[mappedTileIndex];
			bool tileHasOwner = ownerTeamId != SearchSimulationState.k_NoOwnerTeamId && ownerTeamId != SearchSimulationState.k_NotOwnableTeamId;
			if (!tileHasOwner)
			{
				// If the tile doesn't have an owner, move to it will reward the agent with points.
				scoreEarned += (1 + temporalRelevancy * 0.1f) * worthPoints * evaluationParams.TileOwnershipAffinity;
			}

			bool isOwnedByDifferentTeam = tileHasOwner && ownerTeamId != teamId;
			if (isOwnedByDifferentTeam)
			{
				// If the tile is owned by a different team, move to it will greatly reward the agent (increase own score, decrease opponent's score).
				scoreEarned += (1 + temporalRelevancy * 0.2f) * worthPoints * evaluationParams.TileOwnershipAffinity;
			}
		}

		// Evaluate the tile based on the item/powerup
		if (searchSimulationState.TileItems[mappedTileIndex])
		{
			scoreEarned += (1 + temporalRelevancy * 0.3f) * evaluationParams.ItemAffinity;
		}

		// Evaluate if the occupying element is potential prey/predator
		for (int i = 0; i < searchSimulationState.OnTileElementPositions.Length; i++)
		{
			if (i == mappedOnTileElementIndex)
			{
				// Oneself cannot be a prey or predator, skip it.
				continue;
			}

			var onTileElementPosition = searchSimulationState.OnTileElementPositions[i];
			bool isOnTileElementAtThePosition = onTileElementPosition == toPosition;
			bool isOnTileElementAdjacentToThePosition = 
				(onTileElementPosition == (toPosition + Vector2Int.right)) ||
				(onTileElementPosition == (toPosition + Vector2Int.down)) ||
				(onTileElementPosition == (toPosition + Vector2Int.left)) ||
				(onTileElementPosition == (toPosition + Vector2Int.up));
			if (!isOnTileElementAtThePosition && !isOnTileElementAdjacentToThePosition)
			{
				// The OnTileElement is not at the location we are interested in, skip it.
				continue;
			}

			// If the prey is not on the tile, we don't force to chase it.
			float preyFactor = isOnTileElementAtThePosition ? 1.0f : 0.0f;

			// If the predator is adjacent to the tile, we would still want to avoid it.
			float predatorFactor = isOnTileElementAtThePosition ? 1.0f : 0.8f;

			int occupierTeamId = searchSimulationState.OnTileElementTeamIds[i];
			bool isInTheSameTeam = occupierTeamId == teamId;
			if (isInTheSameTeam)
			{
				// The occupier is in the same team as the agent, therefore it is impossible to be a prey or a predator.
				break;
			}

			int opponentPriority = searchSimulationState.OnTileElementPriorities[i];
			int agentPriority = searchSimulationState.OnTileElementPriorities[mappedOnTileElementIndex];

			if (agentPriority < opponentPriority)
			{
				// Prey!
				// The agent can kill this opponent, moving to this position rewards with a kill!
				// We square the temporalRelevancy because we only want to chase the target that is close enough, distant target is more unpredictable.
				scoreEarned += (1 + temporalRelevancy * temporalRelevancy * 0.5f) * preyFactor * evaluationParams.Aggressiveness;
			}
			else
			{
				// Predator!
				// The opponent is possibly dangerous to the agent, moving away from this position to avoid death!
				scoreEarned -= (1 + temporalRelevancy * temporalRelevancy * 0.5f) * predatorFactor * evaluationParams.Cautiousness;
			}
		}

		return scoreEarned;
	}
}
