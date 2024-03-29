using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UpdateTeamGameRankingSystem : IFixedUpdateSystem
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly ElementContext m_ElementContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public UpdateTeamGameRankingSystem(Contexts contexts)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_ElementContext = contexts.Element;
		m_LevelContext = contexts.Level;
		m_MessageContext = contexts.Message;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void FixedUpdate()
	{
		if (m_GameFlowContext.GameFlowEntity.IsGameOver)
		{
			return;
		}

		// Update team game rankings.
		if (m_GameFlowContext.GameFlowEntity.IsScoreObjective)
		{
			var teamEntitiesOrderedByScore = m_TeamInfoGroup.GetEntities().
				OrderBy(teamEntity => -teamEntity.TeamScore.Value).ToArray();

			int previousTeamTileCount = -1;
			for (int teamOrder = 0; teamOrder < teamEntitiesOrderedByScore.Length; teamOrder++)
			{
				var teamEntity = teamEntitiesOrderedByScore[teamOrder];
				int oldTeamGameRanking = teamEntity.TeamGameRanking.Number;
				int newTeamGameRanking = oldTeamGameRanking;
				if (teamEntity.TeamScore.Value == previousTeamTileCount)
				{
					// The same ranking as the last team.
					newTeamGameRanking = teamEntitiesOrderedByScore[teamOrder - 1].TeamGameRanking.Number;
				}
				else
				{
					newTeamGameRanking = teamOrder + 1;
				}

				if (newTeamGameRanking != oldTeamGameRanking)
				{
					// The team has a new ranking, update it!
					teamEntity.ReplaceTeamGameRanking(newTeamGameRanking);
				}

				previousTeamTileCount = teamEntity.TeamScore.Value;
			}
		}
		else if (m_GameFlowContext.GameFlowEntity.IsSurvivalObjective)
		{
			var teamEntitiesOrderedByMemberCount = m_TeamInfoGroup.GetEntities().
				OrderBy(teamEntity => -m_ElementContext.GetNumberOfTeamPlayersAlive(teamEntity.TeamInfo.Id)).ToArray();

			int previousTeamMemberCount = -1;
			for (int teamOrder = 0; teamOrder < teamEntitiesOrderedByMemberCount.Length; teamOrder++)
			{
				var teamEntity = teamEntitiesOrderedByMemberCount[teamOrder];
				int oldTeamGameRanking = teamEntity.TeamGameRanking.Number;
				int newTeamGameRanking = oldTeamGameRanking;
				int teamMemeberCount = m_ElementContext.GetEntitiesWithTeam(teamEntity.TeamInfo.Id).Count;
				if (teamMemeberCount == previousTeamMemberCount)
				{
					// The same ranking as the last team.
					newTeamGameRanking = teamEntitiesOrderedByMemberCount[teamOrder - 1].TeamGameRanking.Number;
				}
				else
				{
					newTeamGameRanking = teamOrder + 1;
				}

				if (newTeamGameRanking != oldTeamGameRanking)
				{
					// The team has a new ranking, update it!
					teamEntity.ReplaceTeamGameRanking(newTeamGameRanking);
				}

				previousTeamMemberCount = teamMemeberCount;
			}
		}
	}
}
