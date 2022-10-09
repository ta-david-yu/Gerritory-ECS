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
	private const int k_SearchDepthLevel = 5;

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
		AIHelper.MinimaxInput minimaxInput = new AIHelper.MinimaxInput()
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

		AIHelper.MinimaxResult result = AIHelper.minimax(minimaxInput, ref searchSimulationState);

		return result.BestAction;
	}
}
