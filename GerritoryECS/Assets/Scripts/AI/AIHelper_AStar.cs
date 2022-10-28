using JCMG.EntitasRedux;
using System;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public static partial class AIHelper
{
	public const int MaxPathLength = 128;

	public struct PathfindingSimulationState
	{
		public enum TileEnterableState
		{
			Vacant,
			OccupiedByPlayer,
			OccupiedByGhost,
			OccupiedByOtherOnTileElement,
			Unenterable,
			NonExistent
		}

		public bool IsAllocated { get; private set; }

		public Allocator AllocatorType;
		public Vector2Int LevelBoundingRectSize;

		/// <summary>
		/// An array of indices for tiles that should be used to access other arrays of data in the struct.
		/// An element with the value of -1 (k_NoTile) means there is no tile at that position.
		/// </summary>
		public NativeArray<int> TileIndices;
		public NativeArray<TileEnterableState> TileEnterables;

		public const int k_NoTile = -1;

		public PathfindingSimulationState AllocateWithContexts(Contexts contexts, Allocator allocator)
		{
			AllocatorType = allocator;

			// Allocate Tile related states
			int numberOfTiles = contexts.Config.GameConfig.value.LevelData.TileDataPairs.Count;
			LevelBoundingRectSize = contexts.Config.GameConfig.value.LevelData.LevelBoundingRectSize;
			TileIndices = new NativeArray<int>(LevelBoundingRectSize.x * LevelBoundingRectSize.y, allocator);
			TileEnterables = new NativeArray<TileEnterableState>(numberOfTiles, allocator);

			IsAllocated = true;

			return this;
		}

		public void InitializeWithContexts(Contexts contexts, int observerOnTileElementId)
		{
			if (!IsAllocated)
			{
				Debug.LogError($"The PathfindingSimulationState is not allocated yet, cannot initialize data!");
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
				int oneDimensionIndex = tilePositionTo1DArrayIndex(tilePosition);
				TileIndices[oneDimensionIndex] = i;

				if (!tile.IsEnterable)
				{
					TileEnterables[i] = TileEnterableState.Unenterable;
				}
			}


			// Update enterable states based on OnTileElements information
			ElementEntity[] relevantElements = contexts.Element.GetGroup
			(
				ElementMatcher.AllOf
				(
					ElementMatcher.OnTileElement
				)
			).GetEntities();
			int numberOfRelevantOnTileElements = relevantElements.Length;

			for (int i = 0; i < numberOfRelevantOnTileElements; i++)
			{
				ElementEntity element = relevantElements[i];
				if (element.OnTileElement.Id == observerOnTileElementId)
				{
					// In pathfinding simulation, the position of the observer itself should be ignored.
					continue;
				}

				if (!element.HasOnTilePosition)
				{
					// Dead OnTileElement is not relevant to our simulation, skip it.
					continue;
				}

				// If the element is not the agent itself, we add a time stagger for the AI to notice the movement.
				// Therefore it feels natural/human-like.
				// Otherwise we would take any MoveOnTile with the progress of more than 0.5f as if it's at the new position.
				float aiNoticeMovementProgress = 0.7f;
				float aiNotiveMovementDelayStagger = 0.25f;
				aiNoticeMovementProgress += UnityEngine.Random.Range(-aiNotiveMovementDelayStagger, aiNotiveMovementDelayStagger);

				Vector2Int elementPosition = new Vector2Int(-1, -1);
				if (element.HasMoveOnTile && element.MoveOnTile.Progress > aiNoticeMovementProgress)
				{
					// If the element is moving, take its destination as the position.
					elementPosition = element.MoveOnTile.ToPosition;
				}
				else
				{
					elementPosition = element.OnTilePosition.Value;
				}

				TileEnterableState tileEnterableState = TileEnterableState.Vacant;
				if (element.HasPlayer)
				{
					tileEnterableState = TileEnterableState.OccupiedByPlayer;
				}
				else if (element.IsGhost)
				{
					tileEnterableState = TileEnterableState.OccupiedByGhost;
				}
				else
				{
					tileEnterableState = TileEnterableState.OccupiedByOtherOnTileElement;
				}
				int tileIndex = GetIndexOfTileAt(elementPosition.ToInt2());
				TileEnterables[tileIndex] = tileEnterableState;
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
			TileEnterables.Dispose();

			IsAllocated = false;
		}

		/// <summary>
		/// Try to get the index of a tile on the given position.
		/// </summary>
		/// <param name="searchSimulationState"></param>
		/// <param name="targetPosition"></param>
		/// <returns>The index of the tile position; otherwise return -1 (k_NoTile) if the position doesn't have a tile.</returns>
		public int GetIndexOfTileAt(int2 targetPosition)
		{
			bool outOfBounds = targetPosition.x < 0 || targetPosition.x >= LevelBoundingRectSize.x ||
							   targetPosition.y < 0 || targetPosition.y >= LevelBoundingRectSize.y;

			if (outOfBounds)
			{
				return PathfindingSimulationState.k_NoTile;
			}

			int tileArrayIndex = targetPosition.x * LevelBoundingRectSize.y + targetPosition.y;
			return TileIndices[tileArrayIndex];
		}

		public TileEnterableState GetEnterableStateOfTileAt(int2 targetPosition)
		{
			int tileIndex = GetIndexOfTileAt(targetPosition);
			if (tileIndex == PathfindingSimulationState.k_NoTile)
			{
				return TileEnterableState.NonExistent;
			}

			return TileEnterables[tileIndex];
		}

		private int tilePositionTo1DArrayIndex(Vector2Int position)
		{
			int tileArrayIndex = position.x * LevelBoundingRectSize.y + position.y;
			return tileArrayIndex;
		}

	}

	public struct AStarResult
	{
		public enum ResultType
		{
			Success,
			NoConnectedPath
		}

		public ResultType Type;
		public int LowestCost;
		public int ValidPathLength;
		public NativeList<int2> Path;
	}
	
	public struct AStarInput
	{
		public int2 StartPosition;
		public int2 EndPosition;
	}

	public struct AStarPathNode : IComparable<AStarPathNode>, IEquatable<AStarPathNode>
	{
		public int2 Position;
		/// <summary>
		/// The cost from the start to this node.
		/// </summary>
		public int GCost;
		/// <summary>
		/// The heuristic cost from this node to the end.
		/// </summary>
		public int HCost;
		/// <summary>
		/// The position of the parent node where this node is entered from.
		/// </summary>
		public int2 ParentPosition;

		public int FCost => GCost + HCost;

		public int CompareTo(AStarPathNode other)
		{
			if (this.FCost < other.FCost)
			{
				return -1;
			}

			if (this.FCost > other.FCost)
			{
				return 1;
			}

			if (this.HCost < other.HCost)
			{
				return -1;
			}

			if (this.HCost > other.HCost)
			{
				return 1;
			}

			return 0;
		}

		public bool Equals(AStarPathNode other)
		{
			return Position.Equals(other.Position);
		}
	}

	public static AStarResult GeneratePathWithAStar(AStarInput input, ref PathfindingSimulationState pathfindingSimulationState, Predicate<PathfindingSimulationState.TileEnterableState> enterablePredicate, int randomSeedIndex)
	{
		int randomSeed = GetRandomValueFromSeedIndex(randomSeedIndex);
		if (input.StartPosition.Equals(input.EndPosition))
		{
			new AStarResult { Type = AStarResult.ResultType.Success, LowestCost = 0, ValidPathLength = 0, Path = new NativeList<int2>() };
		}

		NativeBinaryHeap<AStarPathNode> openNodeList = new NativeBinaryHeap<AStarPathNode>(pathfindingSimulationState.TileEnterables.Length, Allocator.TempJob);
		NativeList<AStarPathNode> closedNodeList = new NativeList<AStarPathNode>(pathfindingSimulationState.TileEnterables.Length, Allocator.TempJob);

		AStarPathNode startNode = new AStarPathNode { Position = input.StartPosition, GCost = 0, HCost = HeuristicDistance(input.StartPosition, input.EndPosition) };
		openNodeList.Add(startNode);

		bool hasConnectedPath = false;
		while (openNodeList.Count != 0)
		{
			AStarPathNode currentNode = openNodeList.RemoveFirst();
			closedNodeList.Add(currentNode);

			if (currentNode.Position.Equals(input.EndPosition))
			{
				// If the visiting node is the end position, means we have found the path for the input. Break the loop!
				hasConnectedPath = true;
				break;
			}

			// Go through all the neighbors of the current node to update their cost & possibly enqueue them.
			var movements = Movement.TypeList;
			int movementStartIndex = randomSeed % Movement.TypeList.Length;
			for (int i = 0; i < movements.Length; i++)
			{
				int movementIndex = (movementStartIndex + i) % movements.Length;
				var movement = movements[movementIndex];

				if (movement == Movement.Type.Stay)
				{
					// We are not interested in Movement.Type.Stay.
					continue;
				}

				int2 moveOffset = Movement.TypeToOffset[movementIndex].ToInt2();
				int2 neighborPosition = currentNode.Position + moveOffset;

				var enterableState = pathfindingSimulationState.GetEnterableStateOfTileAt(neighborPosition);
				bool neighborIsTheEnd = neighborPosition.Equals(input.EndPosition);
				if (!neighborIsTheEnd && !enterablePredicate.Invoke(enterableState))
				{
					// The neighbor tile is not enterable based on the predicate. Skip it.
					continue;
				}

				if (closedNodeList.ContainNodeWithPosition(neighborPosition))
				{
					// This node has already been visited (closed). Skip it.
					continue;
				}

				// Create node for the neighbor with the updated cost value.
				int newNeighborGCost = currentNode.GCost + 1;
				AStarPathNode neighborNode = new AStarPathNode 
				{ 
					Position = neighborPosition, 
					GCost = newNeighborGCost,
					HCost = HeuristicDistance(neighborPosition, input.EndPosition),
					ParentPosition = currentNode.Position
				};
				int existingNodeIndexWithTheSamePosition = openNodeList.IndexOf(neighborNode);
				bool isInOpenNodeList = existingNodeIndexWithTheSamePosition >= 0;

				if (!isInOpenNodeList)
				{
					// A node with the neighbor position has not been recorded in the open list. Enqueue it!
					openNodeList.Add(neighborNode);
					continue;
				}

				if (newNeighborGCost < openNodeList[existingNodeIndexWithTheSamePosition].GCost)
				{
					// If the new g cost is smaller than the recorded one, it means we have reached this node with a shorted path than the previous path.
					// Remove the old node and add the new node with the updated cost!
					openNodeList.RemoveAt(existingNodeIndexWithTheSamePosition);
					openNodeList.Add(neighborNode);

				}
			}
		}

		AStarResult result = new AStarResult();
		if (hasConnectedPath)
		{
			// Traverse through nodes to get the evaluated shortest path.
			NativeList<int2> path = new NativeList<int2>(Allocator.Temp);

			// We traverse from the end position node, which will always be at the end of the closed list.
			AStarPathNode currentNode = closedNodeList[closedNodeList.Length - 1];
			do
			{
				path.Add(currentNode.Position);
				AStarPathNode parentNode;
				if (!closedNodeList.TryGetNodeWithPosition(currentNode.ParentPosition, out parentNode))
				{
					Debug.LogError($"The parent node should always be in the closed node list but it's not found. Something is wrong :(");
					break;
				}
				currentNode = parentNode;
			} while (!currentNode.Position.Equals(input.StartPosition));

			result.Type = AStarResult.ResultType.Success;
			result.LowestCost = closedNodeList[closedNodeList.Length - 1].FCost;
			result.ValidPathLength = path.Length;
			result.Path = path;
		}
		else
		{
			// TODO: recreate a path towards the closest tile to the end location
			// For now we return nothing!
			// ... 
			result.Type = AStarResult.ResultType.NoConnectedPath;
			result.ValidPathLength = 0;
		}

		// Remember to dispose the native containers since they are allocated in unmanaged memory!
		openNodeList.Dispose();
		closedNodeList.Dispose();

		return result;
	}

	public static int HeuristicDistance(int2 start, int2 end)
	{
		return math.abs(start.x - end.x) + math.abs(start.y - end.y);
	}

	public static bool ContainNodeWithPosition(this NativeList<AStarPathNode> list, int2 position)
	{
		foreach (var node in list)
		{
			if (node.Position.Equals(position))
			{
				return true;
			}
		}
		return false;
	}

	public static bool TryGetNodeWithPosition(this NativeList<AStarPathNode> list, int2 position, out AStarPathNode outputNode)
	{
		foreach (var node in list)
		{
			if (node.Position.Equals(position))
			{
				outputNode = node;
				return true;
			}
		}
		outputNode = new AStarPathNode { };
		return false;
	}

	public static int2 ToInt2(this Vector2Int value)
	{
		return new int2() { x = value.x, y = value.y };
	}

	public static Vector2Int ToVector2Int(this int2 value)
	{
		return new Vector2Int() { x = value.x, y = value.y };
	}

	public static void DebugDrawPath(int2[] path, Color color, float duration)
	{
		if (path == null)
		{
			return;
		}

		for (int i = 0; i < path.Length - 1; i++)
		{
			int2 position = path[i];
			int2 parentPosition = path[i + 1];
			Vector3 tileWorldPosition = GameConstants.TilePositionToWorldPosition(position.ToVector2Int());
			Vector3 parentTileWorldPosition = GameConstants.TilePositionToWorldPosition(parentPosition.ToVector2Int());

			Debug.DrawLine(tileWorldPosition, parentTileWorldPosition, color, duration);

			bool isEnd = i == 0;
			if (isEnd)
			{
				DebugDraw.Tile(tileWorldPosition, 0.5f, color, duration);
			}
			else
			{
				DebugDraw.Tile(tileWorldPosition, 0.3f, color, duration);
			}
		}
	}

	/// <summary>
	/// Use this function to sort the order of the given node list when the value at the given element index was changed.
	/// </summary>
	/// <param name="nodeList"></param>
	/// <param name="changedElementIndex"></param>
	public static void BinaryInsertionSortAfterOneElementChanged(this NativeArray<AStarPathNode> nodeList, int changedElementIndex)
	{
		if (changedElementIndex >= nodeList.Length)
		{
			throw new ArgumentOutOfRangeException($"The given index {changedElementIndex} is out of range. The list size is {nodeList.Length}");
		}

		if (nodeList.Length == 1)
		{
			// If there is only one node in the list, no need to sort it :P
			return;
		}

		AStarPathNode changedElement = nodeList[changedElementIndex];

		// Use binary search to find the new in-order index for the changed element.
		// If anything goes wrong, we will just put the element at the end of the list by default (hence the nodeList.Length -1) :P
		int newChangedElementIndex = nodeList.Length - 1;
		int min = 0;
		int max = nodeList.Length;
		while (min < max)
		{
			int mid = min + (max - min) / 2;

			if (mid == changedElementIndex)
			{
				// If the mid index is the same as the target element index, we pick its neighboring element.
				mid = pickValidNeighborIndexOf(mid);
				int pickValidNeighborIndexOf(int index)
				{
					int leftIndex = index - 1;
					int rightIndex = index + 1;

					if (leftIndex >= 0)
					{
						return leftIndex;
					}

					return rightIndex;
				}
			}

			AStarPathNode midElement = nodeList[mid];
			int compareResult = changedElement.CompareTo(midElement);
			if (compareResult > 0)
			{
				min = mid + 1;
			}
			else if (compareResult < 0)
			{
				max = mid - 1;
			}
			else
			{
				newChangedElementIndex = mid;
				break;
			}
		}

		if (min == max)
		{
			if (min < nodeList.Length)
			{
				newChangedElementIndex = min;
			}
			else
			{
				// All the other values are smaller than the changed value, put the changed node to the end.
				newChangedElementIndex = nodeList.Length - 1;
			}
		}
		else if (max < min)
		{
			newChangedElementIndex = max;
		}

		// Now we've known which index should the changed element be moved to,
		// we will start shifting elements between the old index and the new index.
		if (newChangedElementIndex < changedElementIndex)
		{
			// Shift elements from the end iteratively. (right to left)
			for (int i = changedElementIndex; i > newChangedElementIndex; i--)
			{
				nodeList[i] = nodeList[i - 1];
			}

			// Insert the temporary stored changed element to the right location.
			nodeList[newChangedElementIndex] = changedElement;
		}
		else
		{
			// Shift elements from the start iteratively. (left to right)
			for (int i = changedElementIndex; i < newChangedElementIndex; i++)
			{
				nodeList[i] = nodeList[i + 1];
			}

			// Insert the temporary stored changed element to the right location.
			nodeList[newChangedElementIndex] = changedElement;
		}
	}
}
