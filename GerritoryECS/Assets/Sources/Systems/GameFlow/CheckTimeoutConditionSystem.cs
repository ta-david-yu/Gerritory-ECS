using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckTimeoutConditionSystem : IFixedUpdateSystem
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly LevelContext m_LevelContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public CheckTimeoutConditionSystem(Contexts contexts)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_LevelContext = contexts.Level;
	}

	public void FixedUpdate()
	{
		if (m_GameFlowContext.GameFlowEntity.IsGameOver)
		{
			return;
		}

		if (!m_GameFlowContext.GameFlowEntity.HasEndOnTimeout)
		{
			return;
		}

		if (!m_LevelContext.GameInfoEntity.HasGameTimer)
		{
			return;
		}

		if (m_LevelContext.GameInfoEntity.GameTimer.Value < m_GameFlowContext.GameFlowEntity.EndOnTimeout.Time)
		{
			// The timer hasn't reached the timeout value, don't end the game yet.
			return;
		}

		// TODO: Send out a message to indicate game over
		// ...
		Debug.Log("Game Over: Timeout");
		m_GameFlowContext.GameFlowEntity.IsGameOver = true;
	}
}