using JCMG.EntitasRedux;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public sealed class EmitAIInputSystem : IUpdateSystem, ITearDownSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<InputEntity> m_AIInputGroup;

	private const float k_MinimumStayTime = 0.1f;
	private const float k_NextMoveEvaluationTimeOffset = 0.1f;
	private const int k_SearchDepthLevel = 5;

	public EmitAIInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_Contexts = contexts;

		m_AIInputGroup = contexts.Input.GetGroup(InputMatcher.AllOf(InputMatcher.AIInput).NoneOf(InputMatcher.IdleTimer));
	}

	public void Update()
	{
		foreach (var inputEntity in m_AIInputGroup.GetEntities())
		{
			int targetPlayerId = inputEntity.AIInput.TargetPlayerId;
			var elementEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);

			if (!elementEntity.HasOnTilePosition)
			{
				// The entity is currently not in the level, no need to decide the input now.
				continue;
			}

			if (elementEntity.HasMoveOnTile)
			{
				if (inputEntity.HasEvaluatingForMovementInput)
				{
					// The evaluation for the next movement input is already in the process, no need to start a new process.
					continue;
				}

				float totalMoveDuration = elementEntity.GetElementEntityMoveOnTileDuration();
				float timeLeftToCompleteCurrentMove = (1.0f - elementEntity.MoveOnTile.Progress) * totalMoveDuration;
				if (timeLeftToCompleteCurrentMove > k_NextMoveEvaluationTimeOffset)
				{
					// The entity is in the middle of a movement, it's still too early to evaluate the next move.
					continue;
				}

				// Schedule a job to evaluate the next move
				scheduleSearchJobFor(elementEntity, inputEntity);

				continue;
			}

			if (elementEntity.HasMovementInputAction)
			{
				// There is already a buffered input action on the element, skip it.
				continue;
			}

			if (!inputEntity.HasEvaluatingForMovementInput)
			{
				// The agent hasn't decided the next move yet.
				// Schedule a job to evaluate the next move.
				scheduleSearchJobFor(elementEntity, inputEntity);
				continue;
			}

			var evaluationComponent = inputEntity.EvaluatingForMovementInput;
			if (!evaluationComponent.JobHandle.IsCompleted)
			{
				// The job hasn't completed yet, wait for another frame.
				continue;
			}

			// Call complete to make sure the job has actually completed.
			evaluationComponent.JobHandle.Complete();

			// Collect the result and cleanup the component & container
			Movement.Type bestMove = evaluationComponent.Job.ResultContainer[0].BestAction;
			evaluationComponent.Job.ResultContainer.Dispose();
			inputEntity.RemoveEvaluatingForMovementInput();

			if (bestMove == Movement.Type.Stay)
			{
				scheduleSearchJobFor(elementEntity, inputEntity);
				
				// Add an idle timer to avoid move right away after Stay.
				inputEntity.AddIdleTimer(k_MinimumStayTime);
				continue;
			}

			elementEntity.ReplaceMovementInputAction(bestMove, 0.0f);
		}
	}

	public void TearDown()
	{
		foreach (var inputEntity in m_AIInputGroup.GetEntities())
		{
			if (inputEntity.HasEvaluatingForMovementInput)
			{
				inputEntity.EvaluatingForMovementInput.JobHandle.Complete();
				inputEntity.EvaluatingForMovementInput.Job.ResultContainer.Dispose();
			}

			inputEntity.AIInput.SearchSimulationState.Deallocate();
		}
	}

	private void scheduleSearchJobFor(ElementEntity elementEntity, InputEntity inputEntity)
	{
		// Schedule the job to evaluate the next move
		AIHelper.MinimaxInput minimaxInput = new AIHelper.MinimaxInput()
		{
			AgentOnTileElementId = elementEntity.OnTileElement.Id,										// The agent
			AgentTeamId = elementEntity.HasTeam ? elementEntity.Team.Id : -1,
			AgentEvaluationParameters = AIHelper.EvaluationParameters.GetBasicBehaviourParameters(),	// For now we always uses Basic agent behaviour
			CurrentTurnOnTileElementId = elementEntity.OnTileElement.Id,								// We start with the agent's turn
			NumberOfIterationStepsLeft = k_SearchDepthLevel,
			CurrentScore = 0,
			LastMove = Movement.Type.Stay,
			Alpha = int.MinValue,
			Beta = int.MaxValue,
			CallStackCount = 0
		};
		inputEntity.AIInput.SearchSimulationState.InitializeWithContexts(m_Contexts, elementEntity.OnTileElement.Id);
		//inputEntity.AIInput.SearchSimulationState.DebugDrawState();

		NativeArray<AIHelper.MinimaxResult> resultContainer = new NativeArray<AIHelper.MinimaxResult>(1, Allocator.Persistent);
		SearchBestActionWithMinimaxJob job = new SearchBestActionWithMinimaxJob()
		{
			SimulationState = inputEntity.AIInput.SearchSimulationState,
			Input = minimaxInput,
			RandomSeedIndex = UnityEngine.Random.Range(0, 100),
			ResultContainer = resultContainer
		};
		JobHandle jobHandle = job.Schedule();

		inputEntity.AddEvaluatingForMovementInput(job, jobHandle);
	}
}
