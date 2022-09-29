using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmitAIInputSystem : IUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly IGroup<InputEntity> m_AIInputGroup;

	public EmitAIInputSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_AIInputGroup = contexts.Input.GetGroup(InputMatcher.AIInput);
	}

	public void Update()
	{
		foreach (var entity in m_AIInputGroup.GetEntities())
		{
			int targetPlayerId = entity.AIInput.TargetPlayerId;
			var playerEntity = m_ElementContext.GetEntityWithPlayer(targetPlayerId);
			var movement = entity.AIInput.Movement;

			if (playerEntity.HasMoveOnTile)
			{
				continue;
			}

			playerEntity.ReplaceMovementInputAction(movement, 0.0f);
		}
	}
}
