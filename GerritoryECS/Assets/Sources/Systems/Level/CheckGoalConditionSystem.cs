using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckGoalConditionSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public CheckGoalConditionSystem(Contexts contexts)
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

		if (!m_LevelContext.GameInfoEntity.HasEndOnGoalReached)
		{
			return;
		}

		if (m_LevelContext.GameInfo.CurrentHighestTeamScore < m_LevelContext.GameInfoEntity.EndOnGoalReached.GoalScore)
		{
			// The highest team hasn't reached the goal score yet, don't end the game yet.
			return;
		}

		// TODO: Send out a message to indicate game over
		// ...
		Debug.Log("Game Over: Goal Reached");
		m_LevelContext.GameInfoEntity.IsGameOver = true;
	}
}