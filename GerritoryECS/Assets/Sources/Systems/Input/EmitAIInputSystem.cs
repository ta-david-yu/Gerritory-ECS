using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;

public sealed class EmitAIInputSystem : IUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<InputEntity> m_AIInputGroup;

	private const float k_NextMoveEvaluationTimeOffset = 0.1f;
	private const int k_SearchDepthLevel = 1;

	public EmitAIInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_Contexts = contexts;

		m_AIInputGroup = contexts.Input.GetGroup(InputMatcher.AIInput);
	}

	public void Update()
	{
		foreach (var inputEntity in m_AIInputGroup.GetEntities())
		{
			int targetPlayerId = inputEntity.AIInput.TargetPlayerId;
			var playerEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);

			if (!playerEntity.HasOnTilePosition)
			{
				// The entity is currently not in the level, no need to decide the input now.
				continue;
			}

			if (playerEntity.HasMoveOnTile)
			{
				/*
				if (inputEntity.IsEvaluatingForMovementInput)
				{
					// The next movement input has already been decided, no need to evaluate the next move.
					continue;
				}

				float totalMoveDuration = playerEntity.GetElementEntityMoveOnTileDuration();
				float timeLeftToCompleteCurrentMove = (1.0f - playerEntity.MoveOnTile.Progress) * totalMoveDuration;
				if (timeLeftToCompleteCurrentMove > k_NextMoveEvaluationTimeOffset)
				{
					// The player entity is in the middle of a movement, it's still too early to evaluate the next move.
					continue;
				}

				// TODO: Schedule job to evaluate the next move
				// ...
				
				*/

				continue;
			}

			Movement.Type bestMove = Movement.Type.Stay;

			// TODO: Collect job result and apply the result to movement input action
			// ...

			AIHelper.GameSimulationState gameSimulationState = new AIHelper.GameSimulationState();
			gameSimulationState.InitializeWithContexts(m_Contexts, Unity.Collections.Allocator.Temp);

			// TODO: remove this, currently the depth level is only 1.
			float maxScore = float.MinValue;
			var movements = (Movement.Type[])Enum.GetValues(typeof(Movement.Type));
			movements = movements.OrderBy((element) => UnityEngine.Random.Range(0, 100)).ToArray();
			foreach (Movement.Type movement in movements)
			{
				Vector2Int moveOffset = Movement.TypeToOffset[(int)movement];
				Vector2Int currentPosition = playerEntity.OnTilePosition.Value;
				float reward = gameSimulationState.EvaluateScoreEarnedIfOnTileElementMoveTo(playerEntity.OnTileElement.Id, currentPosition + moveOffset, 3);
				//float reward = evaluateScoreEarnedIfOnTileElementMoveToPosition(playerEntity.OnTileElement.Id, currentPosition + moveOffset, 3);

				if (reward > maxScore)
				{
					bestMove = movement;
					maxScore = reward;
				}
			}

			if (bestMove == Movement.Type.Stay)
			{
				continue;
			}

			playerEntity.ReplaceMovementInputAction(bestMove, 0.0f);
		}
	}

	private Movement.Type getNextBestMoveUsingMinMaxForOnTileElement(int onTileElementId)
	{
		MinimaxInput minimaxInput = new MinimaxInput();
		minimaxInput.AgentOnTileElementId = onTileElementId;
		minimaxInput.CurrentOnTileElementId = onTileElementId;
		minimaxInput.NumberOfStepsLeft = k_SearchDepthLevel;
		minimaxInput.CurrentScore = 0;
		minimaxInput.Alpha = int.MinValue;
		minimaxInput.Beta = int.MaxValue;

		MinimaxResult result = minimax(minimaxInput);
		return result.BestAction;
	}

	struct MinimaxInput
	{
		public int AgentOnTileElementId;
		public int CurrentOnTileElementId;
		public int NumberOfStepsLeft;
		public float CurrentScore;
		public float Alpha;
		public float Beta;
	}
	struct MinimaxResult
	{
		public float BestMoveScore;
		public Movement.Type BestAction;
	}
	private MinimaxResult minimax(MinimaxInput input)
	{
		if (input.NumberOfStepsLeft == 0)
		{
			return new MinimaxResult() { BestMoveScore = input.CurrentScore, BestAction = Movement.Type.Stay };
		}

		// TODO:
		// Take the state into consideration.

		if (input.CurrentOnTileElementId == input.AgentOnTileElementId)
		{
			// The agent is taking its move now, maximize the score value!
			float maxScore = float.MinValue;
			var movements = (Movement.Type[]) Enum.GetValues(typeof(Movement.Type));
			foreach (Movement.Type movement in movements)
			{
				Vector2Int moveOffset = Movement.TypeToOffset[(int)movement];
				ElementEntity agentEntity = m_Contexts.Element.GetEntityWithOnTileElement(input.CurrentOnTileElementId);
				Vector2Int currentPosition = agentEntity.OnTilePosition.Value;
				float reward = evaluateScoreEarnedIfOnTileElementMoveToPosition(input.CurrentOnTileElementId, currentPosition + moveOffset, input.NumberOfStepsLeft);
				maxScore = Mathf.Max(maxScore, reward);
			}
		}
		else
		{
			// The other opponent is taking its move now, minimize the score value!
			// TODO: ...
		}

		return new MinimaxResult() { BestMoveScore = input.CurrentScore, BestAction = Movement.Type.Stay };
	}

	/// <summary>
	/// Evaluate the possible score reward if the OnTileElement moves to the given location.
	/// This evaluation takes the move as an independent event, therefore possible changes in the game state that could happen during the move are not considered.
	/// </summary>
	/// <param name="onTileElementId"></param>
	/// <param name="toPosition"></param>
	/// <param name="temporalRelevancy">
	///		How near/revelant time-wise this move is, the higher the value, the higher the resulting score.
	///		For instance, if this is the first move in a search, the value will be higher than that of the second move in the search.
	/// </param>
	/// <returns></returns>
	private float evaluateScoreEarnedIfOnTileElementMoveToPosition(int onTileElementId, Vector2Int toPosition, int temporalRelevancy)
	{
		float scoreEarnedThroughTheMove = 0;

		ElementEntity agentEntity = m_Contexts.Element.GetEntityWithOnTileElement(onTileElementId);
		if (!agentEntity.HasOnTilePosition)
		{
			// If the element is not on a tile (could be dead), the score earned is always zero.
			return 0;
		}

		//Vector2Int currentPosition = agentEntity.OnTilePosition.Value;
		TileEntity tileToMoveTo = m_Contexts.Tile.GetEntityWithTilePosition(toPosition);
		if (tileToMoveTo == null)
		{
			// If the given position doesn't have a tile, the score earned is always zero.
			return 0;
		}

		// Evaluate the tile based on its ownership.
		if (tileToMoveTo.HasOwnable && agentEntity.IsTileOwner)
		{
			int tileWorthPoints = tileToMoveTo.Ownable.WorthPoints;

			if (!tileToMoveTo.HasOwner)
			{
				// If the tile doesn't have an owner, move to it will reward the agent with points.
				scoreEarnedThroughTheMove += (1 + temporalRelevancy * 0.1f) * tileWorthPoints;
			}

			bool isOwnedByDifferenTeam = tileToMoveTo.HasOwner && tileToMoveTo.Owner.OwnerTeamId != agentEntity.Team.Id;
			if (isOwnedByDifferenTeam)
			{
				// If the tile is owned by a different team, move to it will greatly reward the agent (increase own score, decrease opponent's score).
				scoreEarnedThroughTheMove += (1 + temporalRelevancy * 0.2f) * tileWorthPoints;
			}
		}

		// Evaluate the tile based on the item/powerup.
		if (tileToMoveTo.IsItemHolder)
		{
			ItemEntity itemEntity = m_Contexts.Item.GetEntityWithOnTileItem(toPosition);
			if (itemEntity != null)
			{
				// Eating item/powerup is very rewarding.
				scoreEarnedThroughTheMove += (1 + temporalRelevancy * 0.3f);
			}
		}

		// Evaluate the tile based on the potential prey/predator on it.
		List<ElementEntity> onTileEntities = m_ElementContext.GetEntitiesWithOnTilePosition(toPosition).ToList();
		if (onTileEntities.Count > 0)
		{
			ElementEntity occupierEntity = onTileEntities.First();
			bool isInTheSameTeam = occupierEntity.Team.Id == agentEntity.Team.Id;
			if (!isInTheSameTeam)
			{
				int opponentPriority = m_Contexts.GetOnTileElementKillPriority(occupierEntity);
				int agentPriority = m_Contexts.GetOnTileElementKillPriority(agentEntity);

				if (opponentPriority > agentPriority)
				{
					// The agent can kill this opponent, moving to this position rewards with a kill!
					// We square the temporalRelevancy because we only want to chase the target that is close enough, distant target is more unpredictable.
					scoreEarnedThroughTheMove += (1 + temporalRelevancy * temporalRelevancy * 0.5f);
				}
				else
				{
					// The opponent is possibly dangerous to the agent, moving away from this position to avoid death!
					scoreEarnedThroughTheMove -= (1 + temporalRelevancy * temporalRelevancy * 0.5f);
				}
			}
		}

		return scoreEarnedThroughTheMove;
	}
}
