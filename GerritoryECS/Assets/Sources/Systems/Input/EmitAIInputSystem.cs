using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public sealed class EmitAIInputSystem : IUpdateSystem, ITearDownSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<InputEntity> m_AIInputGroup;

	private const float k_NextMoveEvaluationTimeOffset = 0.1f;
	private const int k_SearchDepthLevel = 2;

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

			Movement.Type bestMove = getNextBestMoveUsingMinMaxForOnTileElement(playerEntity, ref inputEntity.AIInput.SearchSimulationState);

			if (bestMove == Movement.Type.Stay)
			{
				continue;
			}

			playerEntity.ReplaceMovementInputAction(bestMove, 0.0f);
		}
	}


	public void TearDown()
	{
		foreach (var inputEntity in m_AIInputGroup.GetEntities())
		{
			inputEntity.AIInput.SearchSimulationState.Deallocate();
		}
	}

	private Movement.Type getNextBestMoveUsingMinMaxForOnTileElement(ElementEntity elementEntity, ref AIHelper.SearchSimulationState searchSimulationState)
	{
		MinimaxInput minimaxInput = new MinimaxInput()
		{
			AgentOnTileElementId = elementEntity.OnTileElement.Id,					// The agent
			AgentTeamId = elementEntity.HasTeam? elementEntity.Team.Id : -1,
			CurrentTurnOnTileElementId = elementEntity.OnTileElement.Id,			// We start with the agent's turn
			NumberOfIterationStepsLeft = k_SearchDepthLevel,
			CurrentScore = 0,
			LastMove = Movement.Type.Stay,
			Alpha = int.MinValue,
			Beta = int.MaxValue,
			CallStackCount = 0
		};

		searchSimulationState.InitializeWithContexts(m_Contexts);

		MinimaxResult result = minimax(minimaxInput, ref searchSimulationState);

		return result.BestAction;
	}

	struct MinimaxInput
	{
		public int AgentOnTileElementId;
		public int AgentTeamId;
		public int CurrentTurnOnTileElementId;
		public int NumberOfIterationStepsLeft;
		public float CurrentScore;
		public Movement.Type LastMove;
		public float Alpha;
		public float Beta;
		public int CallStackCount;
	}
	struct MinimaxResult
	{
		public float BestActionScore;
		public Movement.Type BestAction;
	}
	private static MinimaxResult minimax(MinimaxInput input, ref AIHelper.SearchSimulationState searchSimulationState)
	{
		if (input.NumberOfIterationStepsLeft == 0)
		{
			return new MinimaxResult() { BestActionScore = input.CurrentScore, BestAction = input.LastMove };
		}

		int mappedElementIndex = searchSimulationState.GetIndexOfOnTileElementWithId(input.CurrentTurnOnTileElementId);
		int teamId = searchSimulationState.OnTileElementTeamIds[mappedElementIndex];
		Vector2Int currPosition = searchSimulationState.OnTileElementPositions[mappedElementIndex];

		bool isTheAgent = input.CurrentTurnOnTileElementId == input.AgentOnTileElementId;
		bool isInTheSameTeamAsAgent = teamId == input.AgentTeamId;
		bool isFriendlyTurn = isTheAgent || (teamId != AIHelper.SearchSimulationState.k_NoTeam && isInTheSameTeamAsAgent);

		float bestActionScore = isFriendlyTurn ? float.MinValue : float.MaxValue;
		Movement.Type bestAction = Movement.Type.Stay;

		// Go through all the possible moves/actions recursively to see which one is the best action.
		var movements = Movement.TypeList;
		int movementStartIndex = UnityEngine.Random.Range(0, movements.Length);
		for (int i = 0; i < movements.Length; i++)
		{
			var movement = movements[(movementStartIndex + i) % movements.Length];

			Vector2Int moveOffset = Movement.TypeToOffset[(int)movement];
			Vector2Int nextPosition = currPosition + moveOffset;
			int tileIndex = searchSimulationState.GetIndexOfTileAt(nextPosition);
			if (tileIndex == AIHelper.SearchSimulationState.k_NoTile)
			{
				// There is no tile at the position, skip it.
				continue;
			}

			// Evaluate the reward earned from the action.
			float scoreEarnedWithTheAction = searchSimulationState.EvaluateScoreEarnedIfOnTileElementMoveTo
			(
				input.CurrentTurnOnTileElementId, 
				nextPosition,  
				isFriendlyTurn? AIHelper.EvaluationParameters.GetBasicBehaviourParameters() : AIHelper.EvaluationParameters.GetPeacefulBehaviourParameters(), 
				input.NumberOfIterationStepsLeft
			);

			// Depending on whose turn it is, the score earned could either be a reward or a penalty
			float scoreAfterTakingTheAction = input.CurrentScore + (isFriendlyTurn? scoreEarnedWithTheAction : -scoreEarnedWithTheAction);

			// Update the search simulation state as if the action is done.
			AIHelper.SearchSimulationState.OnTileElementAction simulationAction = new AIHelper.SearchSimulationState.OnTileElementAction()
			{
				OnTileElementId = input.CurrentTurnOnTileElementId,
				MoveToPosition = nextPosition
			};
			simulationAction.Apply(ref searchSimulationState);

			// Select the next OnTileElement to do the action simulation.
			int numberOfRelevantOnTileElements = searchSimulationState.OnTileElementIds.Length;
			int nextOnTileElementIndex = (mappedElementIndex + 1) % numberOfRelevantOnTileElements;
			while (searchSimulationState.AreOnTileElementsDead[nextOnTileElementIndex])
			{
				// Loop to find an element that is not dead
				nextOnTileElementIndex = (nextOnTileElementIndex + 1) % numberOfRelevantOnTileElements;
			}

			int nextOnTileElementId = searchSimulationState.OnTileElementIds[nextOnTileElementIndex];

			int iterationStepsLeft = input.NumberOfIterationStepsLeft;
			if (nextOnTileElementId == input.AgentOnTileElementId)
			{
				// Decrement the search iteration depth because we've circled through all the elements' turns and got back to the main agent (or we have just started).
				iterationStepsLeft -= 1;
			}

			MinimaxInput nextMinimaxInput = new MinimaxInput()
			{
				AgentOnTileElementId = input.AgentOnTileElementId,
				AgentTeamId = input.AgentTeamId,
				CurrentTurnOnTileElementId = nextOnTileElementId,
				NumberOfIterationStepsLeft = iterationStepsLeft,
				CurrentScore = scoreAfterTakingTheAction,
				LastMove = movement,
				Alpha = input.Alpha,
				Beta = input.Beta,
				CallStackCount = input.CallStackCount + 1
			};

			// Search the next best move.
			MinimaxResult minimaxResult = minimax(nextMinimaxInput, ref searchSimulationState);

			float finalScoreAfterTakingTheAction = minimaxResult.BestActionScore;

			if (isFriendlyTurn)
			{
				if (finalScoreAfterTakingTheAction > bestActionScore)
				{
					bestActionScore = finalScoreAfterTakingTheAction;
					bestAction = movement;
				}

				if (finalScoreAfterTakingTheAction > input.Alpha)
				{
					// If the current turn is the main agent's/friendly turn, it would always try to maximize the score.
					// Therefore since the best score from the search is better than alpha, it becomes the new alpha.
					input.Alpha = finalScoreAfterTakingTheAction;
				}
			}
			else
			{
				if (finalScoreAfterTakingTheAction < bestActionScore)
				{
					bestActionScore = finalScoreAfterTakingTheAction;
					bestAction = movement;
				}

				if (finalScoreAfterTakingTheAction < input.Beta)
				{
					// If the current turn is the opponent's turn, it would always try to minimize the score.
					// Therefore since the best score from the search is worse than beta, it becomes the new beta.
					input.Beta = finalScoreAfterTakingTheAction;
				}
			}

			// Returned from the recursive call stack, revert the simulation state.
			simulationAction.Revert(ref searchSimulationState);

			if (input.Beta <= input.Alpha)
			{
				// The beta being less than alpha means there is already a better move for opponents (i.e. minimizes the agent's score more) in other actions;
				// Therefore there is no need to search more on this level.
				break;
			}
		}

		if (input.CallStackCount == 0)
		{
			// TODO: add more randomness
		}

		return new MinimaxResult() { BestActionScore = bestActionScore, BestAction = bestAction };
	}
}
