using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using static AIHelper;

public sealed class EmitChaseNearestOnTileElementVictimInputSystem : IUpdateSystem, ITearDownSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<InputEntity> m_NavigateToPositionInputGroup;
	private readonly IGroup<ElementEntity> m_VictimCandidatesGroup;

	private const float k_MaximumStayTime = 0.3f;
	private const float k_MinimumStayTime = 0.15f;

	public EmitChaseNearestOnTileElementVictimInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_Contexts = contexts;

		m_NavigateToPositionInputGroup = contexts.Input.GetGroup(InputMatcher.AllOf(InputMatcher.ChaseNearestOnTileElementVictimInput));
		m_VictimCandidatesGroup = contexts.Element.GetGroup(ElementMatcher.AllOf(ElementMatcher.OnTileElement, ElementMatcher.OnTilePosition, ElementMatcher.CanBeDead).NoneOf(ElementMatcher.Dead));
	}

	public void Update()
	{
		foreach (var inputEntity in m_NavigateToPositionInputGroup.GetEntities())
		{
			if (inputEntity.HasIdleTimer)
			{
				// The input entity is still idling, skip it.
				continue;
			}

			int controllingElementId = inputEntity.ChaseNearestOnTileElementVictimInput.ControllingElementId;
			var elementEntity = m_ElementContext.GetEntityWithOnTileElement(controllingElementId);

			if (!elementEntity.HasOnTilePosition)
			{
				// The entity is currently not in the level, no need to decide the input now.
				continue;
			}

			if (elementEntity.HasMoveOnTile)
			{
				// The entity is currently moving, no need to decide the input now.
				continue;
			}

			if (elementEntity.HasMovementInputAction)
			{
				// There is already a buffered input action on the element, skip it.
				continue;
			}

			// Initialize pathfinding simulation state/data.
			var inputComponent = inputEntity.ChaseNearestOnTileElementVictimInput;
			inputComponent.PathfindingSimulationState.InitializeWithContexts(m_Contexts, elementEntity.OnTileElement.Id);

			// Cache chaser information.
			int chaserId = elementEntity.OnTileElement.Id;
			int2 startPosition = elementEntity.OnTilePosition.Value.ToInt2();

			const int k_NoVictimElementId = -1;
			int nearestVictimElementId = k_NoVictimElementId;
			int nearestVictimGameRanking = int.MaxValue;
			int distanceToNearestVictim = int.MaxValue;
			int2 firstPositionOfPathToNearestVictim = startPosition;
			int2[] debugPathToNearestVictimCache = null;
			int debugPathCost = -1;
			foreach (var candidateEntity in m_VictimCandidatesGroup.GetEntities())
			{
				int candidateId = candidateEntity.OnTileElement.Id;
				if (candidateId == chaserId)
				{
					// You cannot chase yourself :P. Skip it!
					continue;
				}

				bool canBeVictim = m_Contexts.CanStepOnVictim(elementEntity, candidateEntity);
				if (!canBeVictim)
				{
					// This candidate is not killable. Skip it!
					continue;
				}

				// TODO: this needs to be cached in a different struct
				// ...
				int2 targetPosition = inputComponent.PathfindingSimulationState.OnTileElementPositions[candidateId];
				const int k_NoTeamRanking = 99;
				int candidateTeamGameRanking = candidateEntity.HasTeam ? m_Contexts.Level.GetEntityWithTeamInfo(candidateEntity.Team.Id).TeamGameRanking.Number : k_NoTeamRanking;

				int heuristicDistance = AIHelper.HeuristicDistance(startPosition, targetPosition);
				if (heuristicDistance > inputEntity.ChaseNearestOnTileElementVictimInput.MaxSearchHeuristicDistance)
				{
					// This candidate is out of range. Skip it!
					continue;
				}

				// Generate the shortest path to the target candidate
				AIHelper.AStarInput aStarInput = new AIHelper.AStarInput
				{
					StartPosition = startPosition,
					EndPosition = targetPosition
				};
				var aStarResult = AIHelper.GeneratePathWithAStar
				(
					aStarInput, 
					ref inputEntity.ChaseNearestOnTileElementVictimInput.PathfindingSimulationState, 
					(AIHelper.PathfindingSimulationState.TileEnterableState enterableState) =>
					{
						// TODO: do more logic checking to ignore certain types of OnTileElement
						// ...
						return enterableState == AIHelper.PathfindingSimulationState.TileEnterableState.Vacant;
					}, 
					randomSeedIndex: UnityEngine.Random.Range(0, 100)
				);
				
				if (aStarResult.Type == AIHelper.AStarResult.ResultType.NoConnectedPath)
				{
					// Cannot reach the target candidate (no path). Skip it!
					DebugDraw.Cross(GameConstants.TilePositionToWorldPosition(startPosition.ToVector2Int()), 0.5f, Color.red, 0.2f);
					continue;
				}

				bool isBetterVictimCandidate = false;

				int distanceToCandidate = aStarResult.ValidPathLength;
				if (distanceToCandidate < distanceToNearestVictim)
				{
					// Found a target that is closer, target it as the victim instead!
					isBetterVictimCandidate = true;
				}
				else if (distanceToCandidate == distanceToNearestVictim)
				{
					// If two candidates have the same distance, we pick one that has better ranking number!
					if (candidateTeamGameRanking < nearestVictimGameRanking)
					{
						isBetterVictimCandidate = true;
					}
				}

				if (isBetterVictimCandidate)
				{
					// Update relevant nearest victim info.
					nearestVictimElementId = candidateId;
					distanceToNearestVictim = distanceToCandidate;
					firstPositionOfPathToNearestVictim = aStarResult.Path.Length > 0 ? aStarResult.Path[aStarResult.Path.Length - 1] : startPosition;


					// Debug draw the shortest path to each valid candidate.
					debugPathToNearestVictimCache = aStarResult.Path.ToArray();
					debugPathCost = aStarResult.LowestCost;
				}
			}

			// Debug draw :)))
			Debug.Log($"Cost: {debugPathCost}");
			AIHelper.DebugDrawPath(debugPathToNearestVictimCache, Color.blue, GameConstants.MoveOnTileDuration);

			// Get the actual movement to take from the calculated path.
			Movement.Type movementToTarget = Movement.Type.Stay;
			if (!Movement.TryGetTypeFromOffset(firstPositionOfPathToNearestVictim.ToVector2Int() - startPosition.ToVector2Int(), out movementToTarget))
			{
				Debug.LogError($"The first step is not reachable with one movement: {startPosition} -> {firstPositionOfPathToNearestVictim}.");
			}

			if (nearestVictimElementId == k_NoVictimElementId)
			{
				// When we couldn't find a proper victim, we will just pick a random movement.
				int randomNumber = AIHelper.GetRandomValueFromSeedIndex(UnityEngine.Random.Range(0, 100));
				int movementIndex = randomNumber % Movement.TypeList.Length;
				movementToTarget = Movement.TypeList[movementIndex];
				firstPositionOfPathToNearestVictim = startPosition + Movement.TypeToOffset[movementIndex].ToInt2();
			}

			if (movementToTarget == Movement.Type.Stay)
			{
				// Add an idle timer to avoid move right away after Stay.
				inputEntity.AddIdleTimer(UnityEngine.Random.Range(k_MinimumStayTime, k_MaximumStayTime));
				continue;
			}

			elementEntity.ReplaceMovementInputAction(movementToTarget, 0.0f);
		}
	}

	public void TearDown()
	{
		foreach (var inputEntity in m_NavigateToPositionInputGroup.GetEntities())
		{
			/*
			if (inputEntity.HasEvaluatingForMovementInput)
			{
				inputEntity.EvaluatingForMovementInput.JobHandle.Complete();
				inputEntity.EvaluatingForMovementInput.Job.ResultContainer.Dispose();
			}*/

			inputEntity.ChaseNearestOnTileElementVictimInput.PathfindingSimulationState.Deallocate();
		}
	}

}
