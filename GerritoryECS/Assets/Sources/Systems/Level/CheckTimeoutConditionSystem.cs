using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckTimeoutConditionSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public CheckTimeoutConditionSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_LevelContext = contexts.Level;
		m_MessageContext = contexts.Message;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void FixedUpdate()
	{
		if (m_LevelContext.GameInfoEntity.IsGameOver)
		{
			return;
		}

		if (!m_LevelContext.GameInfoEntity.HasEndOnTimeout)
		{
			return;
		}

		if (m_LevelContext.GameInfoEntity.GameTimer.Value < m_LevelContext.GameInfoEntity.EndOnTimeout.Time)
		{
			// The timer hasn't reached the timeout value yet, don't end the game yet.
			return;
		}

		// TODO: Send out a message to indicate game over
		// ...
		Debug.Log("Game Over: Timeout");
		m_LevelContext.GameInfoEntity.IsGameOver = true;
	}
}