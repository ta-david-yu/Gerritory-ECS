using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameInfoSystem : IInitializeSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public SetupGameInfoSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
		m_Contexts = contexts;

		m_TeamInfoGroup = m_LevelContext.GetGroup(LevelMatcher.TeamInfo);
	}

	public void Initialize()
	{
		Dictionary<int, int> numbersOfTeamMembers = new Dictionary<int, int>();
		int currentHighestNumberOfTeamMemebers = 0;

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
			int numberOfMembers = 0;
			if (numbersOfTeamMembers.TryGetValue(playerConfig.TeamId, out int number))
			{
				numberOfMembers = number + 1;
				numbersOfTeamMembers[playerConfig.TeamId] = numberOfMembers;
			}
			else
			{
				numberOfMembers = 1;
				numbersOfTeamMembers.Add(playerConfig.TeamId, numberOfMembers);
			}

			if (numberOfMembers > currentHighestNumberOfTeamMemebers)
			{
				currentHighestNumberOfTeamMemebers = numberOfMembers;
			}
		}

		// Calculate highest objective value
		LevelEntity gameInfoEntity = m_LevelContext.SetGameInfo(newCurrentHighestTeamScore: 0, newCurrentHighestTeamMemberCount: currentHighestNumberOfTeamMemebers);

		// Create game info view controller
		IEntityCreationEventController gameInfoViewController = m_ConfigContext.GameConfig.value.GameInfoViewFactory.CreateGameInfoViewController();
		gameInfoViewController.OnEntityCreated(m_Contexts, gameInfoEntity);
		gameInfoViewController.OnComponentsAdded(m_Contexts, gameInfoEntity);
	}
}
