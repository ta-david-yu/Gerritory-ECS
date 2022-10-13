using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public sealed class UpdateStateTimerSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly ItemContext m_ItemContex;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<PlayerStateEntity> m_StateTimerGroup;

	public UpdateStateTimerSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_ItemContex = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;
		m_Contexts = contexts;

		m_StateTimerGroup = m_PlayerStateContext.GetGroup(PlayerStateMatcher.AllOf(PlayerStateMatcher.State, PlayerStateMatcher.Timer));
	}

	public void FixedUpdate()
	{
		foreach (var stateEntity in m_StateTimerGroup.GetEntities())
		{
			float stateTimer = stateEntity.Timer.Value;
			if (stateTimer < 0)
			{
				// The state timer is up, remove the state entity.
				m_Contexts.DestroyPlayerStateEntity(stateEntity);
			}
			else
			{
				stateTimer -= Time.fixedDeltaTime;
				stateEntity.ReplaceTimer(stateTimer);
			}
		}
	}
}
