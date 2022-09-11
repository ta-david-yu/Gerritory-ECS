using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpdateStateTimerSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly ItemContext m_ItemContex;
	private readonly PlayerStateContext m_PlayerStateContext;

	private readonly IGroup<PlayerStateEntity> m_StateTimerGroup;

	public UpdateStateTimerSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_ItemContex = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;

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
				stateEntity.Destroy();
			}
			else
			{
				stateTimer -= Time.fixedDeltaTime;
				stateEntity.ReplaceTimer(stateTimer);
			}
		}
	}
}
