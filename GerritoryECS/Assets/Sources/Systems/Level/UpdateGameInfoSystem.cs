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
		int highestTeamScore = -1;
		// Update highest team score.
		foreach (var teamEntity in m_TeamInfoGroup)
		{
			if (teamEntity.TeamScore.Value >= highestTeamScore)
			{
				highestTeamScore = teamEntity.TeamScore.Value;
			}
		}
		m_LevelContext.ReplaceGameInfo(newCurrentHighestTeamScore: highestTeamScore);

		// Update team game rankings.
		var teamEntitiesOrderedByScore = m_TeamInfoGroup.GetEntities().OrderBy(teamEntity => -teamEntity.TeamScore.Value).ToArray();
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
}
