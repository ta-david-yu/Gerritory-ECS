using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckEliminationConditionSystem : IFixedUpdateSystem
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly ElementContext m_ElementContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public CheckEliminationConditionSystem(Contexts contexts)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_ElementContext = contexts.Element;
		m_LevelContext = contexts.Level;
		m_MessageContext = contexts.Message;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void FixedUpdate()
	{
		if (!m_GameFlowContext.GameFlowEntity.IsPlaying)
		{
			return;
		}

		if (m_GameFlowContext.GameFlowEntity.IsGameOver)
		{
			return;
		}

		if (!m_GameFlowContext.GameFlowEntity.HasEndOnEliminated)
		{
			return;
		}

		int numberOfTeamsShouldBeLeft = m_GameFlowContext.GameFlowEntity.EndOnEliminated.NumberOfTeamsShouldBeLeft;
		int numberOfAliveTeams = 0;
		foreach (LevelEntity teamEntity in m_TeamInfoGroup)
		{
			int aliveTeamMemberCount = m_ElementContext.GetNumberOfTeamPlayersAlive(teamEntity.TeamInfo.Id);
			if (aliveTeamMemberCount > 0)
			{
				numberOfAliveTeams += 1;
			}
		}

		if (numberOfAliveTeams > numberOfTeamsShouldBeLeft)
		{
			// The number of alive teams is still bigger than the threshold, don't end the game yet.
			return;
		}

		// TODO: Send out a message to indicate game over
		// ...
		Debug.Log("Game Over: Elimination");
		m_GameFlowContext.GameFlowEntity.IsGameOver = true;
	}
}
