using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UpdateGameInfoSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly LevelContext m_LevelContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public UpdateGameInfoSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_LevelContext = contexts.Level;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void FixedUpdate()
	{
		int highestTeamScore = -1;
		int highestTeamMemberCount = -1;
		foreach (var teamEntity in m_TeamInfoGroup)
		{
			if (teamEntity.TeamScore.Value >= highestTeamScore)
			{
				highestTeamScore = teamEntity.TeamScore.Value;
			}

			int teamMemberCount = m_ElementContext.GetNumberOfTeamPlayersAlive(teamEntity.TeamInfo.Id);
			if (teamMemberCount >= highestTeamMemberCount)
			{
				highestTeamMemberCount = teamMemberCount;
			}
		}
		m_LevelContext.ReplaceGameInfo(newCurrentHighestTeamScore: highestTeamScore, newCurrentHighestTeamMemberCount: highestTeamMemberCount);
	}
}
