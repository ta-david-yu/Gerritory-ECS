using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class AIHelper
{
	public struct MinimaxInput
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
	public struct MinimaxResult
	{
		public float BestActionScore;
		public Movement.Type BestAction;
	}
	public static MinimaxResult minimax(MinimaxInput input, ref AIHelper.SearchSimulationState searchSimulationState)
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
				isFriendlyTurn ? AIHelper.EvaluationParameters.GetBasicBehaviourParameters() : AIHelper.EvaluationParameters.GetPeacefulBehaviourParameters(),
				input.NumberOfIterationStepsLeft
			);

			// Depending on whose turn it is, the score earned could either be a reward or a penalty
			float scoreAfterTakingTheAction = input.CurrentScore + (isFriendlyTurn ? scoreEarnedWithTheAction : -scoreEarnedWithTheAction);

			// Update the search simulation state as if the action is done.
			AIHelper.SearchSimulationState.OnTileElementAction simulationAction = new AIHelper.SearchSimulationState.OnTileElementAction()
			{
				OnTileElementId = input.CurrentTurnOnTileElementId,
				MoveToPosition = nextPosition
			};
			simulationAction.Apply(ref searchSimulationState);

			// Select the next OnTileElement to do the action simulation.
			// Normally in minimax you would choose another OnTileElement other than the agent itself,
			// Something like this:
			// 
			//	int numberOfRelevantOnTileElements = searchSimulationState.OnTileElementIds.Length;
			//	int nextOnTileElementIndex = (mappedElementIndex + 1) % numberOfRelevantOnTileElements;
			//	while (searchSimulationState.AreOnTileElementsDead[nextOnTileElementIndex])
			//	{
			//		nextOnTileElementIndex = (nextOnTileElementIndex + 1) % numberOfRelevantOnTileElements;
			//	}
			// 
			// but it turns out that, since our game is real-time action, assuming players would take turn to move doesn't help the AI to search better.
			// Therefore we assume all the other OnTileElements are static threats/rewards as if they wouldn't move during the AI search simulation.
			// It's more performant + the resulted behaviour is similar to the previous implementation.
			int nextOnTileElementIndex = mappedElementIndex;
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
