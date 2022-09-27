using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameInfoSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;

	public SetupGameInfoSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		m_LevelContext.SetGameInfo(newCurrentHighestTeamScore: 0);

		foreach (var playerConfig in m_ConfigContext.GameConfig.value.PlayerGameConfigs)
		{
			// Create team entity if there isn't one with the given id.
			if (m_LevelContext.GetEntityWithTeamInfo(playerConfig.TeamId) == null)
			{
				LevelEntity teamEntity = m_LevelContext.CreateEntity();
				teamEntity.AddTeamInfo(playerConfig.TeamId);
				teamEntity.AddTeamScore(0);
				teamEntity.AddTeamGameRanking(1);
			}
		}
	}
}
