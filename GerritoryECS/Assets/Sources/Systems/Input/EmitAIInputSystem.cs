using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmitAIInputSystem : IUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly IGroup<InputEntity> m_AIInputGroup;

	private const float k_NextMoveEvaluationTimeOffset = 0.1f;

	public EmitAIInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_AIInputGroup = contexts.Input.GetGroup(InputMatcher.AIInput);
	}

	public void Update()
	{
		foreach (var inputEntity in m_AIInputGroup.GetEntities())
		{
			int targetPlayerId = inputEntity.AIInput.TargetPlayerId;
			var playerEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);

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

			// TODO: Collect job result and apply the result to movement input action
			// ...

			var movement = (Movement.Type)UnityEngine.Random.Range(0, 4);
			playerEntity.ReplaceMovementInputAction(Movement.Type.Right, 0.0f);
		}
	}
}
