using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpdateLeadingTeamSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	private int[] m_CachedLeadingTeamIds = new int[GameConstants.MaxTeamCount];

	public UpdateLeadingTeamSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void FixedUpdate()
	{
		LevelEntity gameInfoEntity = m_LevelContext.GameInfoEntity;

		if (!gameInfoEntity.HasGameTimer)
		{
			// The game hasn't started yet, skip the logic.
			return;
		}

		if (gameInfoEntity.GameTimer.Value < GameConstants.HasLeadingTeamTime)
		{
			// The game has just started, don't update leading team information.
			return;
		}

		int previousLeadingTeamId = gameInfoEntity.HasLeadingTeam ? gameInfoEntity.LeadingTeam.TeamId : -1;
		int numberOfLeadingTeams = 0;
		int newMajorLeadingTeamId = -1;

		// Collect all teams that have a game ranking of 1
		foreach (var teamEntity in m_TeamInfoGroup.GetEntities())
		{
			if (teamEntity.TeamGameRanking.Number != 1)
			{
				continue;
			}

			m_CachedLeadingTeamIds[numberOfLeadingTeams] = teamEntity.TeamInfo.Id;
			numberOfLeadingTeams++;
		}

		if (numberOfLeadingTeams == 0)
		{
			Debug.LogError("There should be at least one leading team! There is something wrong :P");
			return;
		}

		if (numberOfLeadingTeams == 1)
		{
			// Set the leading team value to the new team id if there is only one leading team.
			newMajorLeadingTeamId = m_CachedLeadingTeamIds[0];
		}
		else
		{
			bool isPreviousLeadingTeamStillLeading = false;
			for (int i = 0; i < numberOfLeadingTeams; i++)
			{
				var teamId = m_CachedLeadingTeamIds[i];
				if (teamId != previousLeadingTeamId)
				{
					continue;
				}

				isPreviousLeadingTeamStillLeading = true;
			}

			if (isPreviousLeadingTeamStillLeading)
			{
				// The previous leading team will continuously be the major leading team even if there are more than one leading teams.
				newMajorLeadingTeamId = previousLeadingTeamId;
			}
			else
			{
				// Pick the first team in the list as the major leading team.
				newMajorLeadingTeamId = m_CachedLeadingTeamIds[0];
			}
		}

		if (newMajorLeadingTeamId != previousLeadingTeamId)
		{
			gameInfoEntity.ReplaceLeadingTeam(newMajorLeadingTeamId);
		}
	}
}
