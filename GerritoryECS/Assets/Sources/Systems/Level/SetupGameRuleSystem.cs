using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SetupGameRuleSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;

	public SetupGameRuleSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		GameObjective gameObjective = m_ConfigContext.GameConfig.value.Objective;
		switch (gameObjective)
		{
			case GameObjective.Survival:
				m_LevelContext.GameInfoEntity.IsSurvivalObjective = true;
				break;
			case GameObjective.Score:
				m_LevelContext.GameInfoEntity.IsScoreObjective = true;
				break;
		}

		GameEndingCondition endingCondition = m_ConfigContext.GameConfig.value.EndingCondition;

		if ((int) endingCondition == 0)
		{
			Debug.LogWarning($"The ending condition is not set to any of the given conditions. The game won't be over unless GameOverComponent is added manually.");
		}

		if (endingCondition.HasFlag(GameEndingCondition.Timeout))
		{
			m_LevelContext.GameInfoEntity.AddEndOnTimeout(m_ConfigContext.GameConfig.value.Timeout);
		}

		if (endingCondition.HasFlag(GameEndingCondition.Goal))
		{
			m_LevelContext.GameInfoEntity.AddEndOnGoalReached(m_ConfigContext.GameConfig.value.GoalScore);
		}

		if (endingCondition.HasFlag(GameEndingCondition.Elimination))
		{
			m_LevelContext.GameInfoEntity.AddEndOnEliminated(m_ConfigContext.GameConfig.value.NumberOfTeamsShouldBeLeft);
		}
	}
}
