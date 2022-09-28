using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UpdateGameInfoSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public UpdateGameInfoSystem(Contexts contexts)
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

		int highestTeamScore = -1;
		int highestTeamMemberCount = -1;
		foreach (var teamEntity in m_TeamInfoGroup)
		{
			if (teamEntity.TeamScore.Value >= highestTeamScore)
			{
				highestTeamScore = teamEntity.TeamScore.Value;
			}

			int teamMemberCount = m_GameContext.GetNumberOfTeamPlayersAlive(teamEntity.TeamInfo.Id);
			if (teamMemberCount >= highestTeamMemberCount)
			{
				highestTeamMemberCount = teamMemberCount;
			}
		}
		m_LevelContext.ReplaceGameInfo(newCurrentHighestTeamScore: highestTeamScore, newCurrentHighestTeamMemberCount: highestTeamMemberCount);
		m_LevelContext.GameInfoEntity.ReplaceGameTimer(m_LevelContext.GameInfoEntity.GameTimer.Value + Time.fixedDeltaTime);
	}
}
