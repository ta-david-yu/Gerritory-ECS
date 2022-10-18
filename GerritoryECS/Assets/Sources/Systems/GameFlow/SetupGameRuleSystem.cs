using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SetupGameRuleSystem : IInitializeSystem
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;

	public SetupGameRuleSystem(Contexts contexts)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		GameObjective gameObjective = m_ConfigContext.GameConfig.value.LevelData.Objective;
		switch (gameObjective)
		{
			case GameObjective.Survival:
				m_GameFlowContext.GameFlowEntity.IsSurvivalObjective = true;
				break;
			case GameObjective.Score:
				m_GameFlowContext.GameFlowEntity.IsScoreObjective = true;
				break;
		}

		GameEndingCondition endingCondition = m_ConfigContext.GameConfig.value.LevelData.EndingCondition;

		if ((int) endingCondition == 0)
		{
			Debug.LogWarning($"The ending condition is not set to any of the given conditions. The game won't be over unless GameOverComponent is added manually.");
		}

		if (endingCondition.HasFlag(GameEndingCondition.Timeout))
		{
			m_GameFlowContext.GameFlowEntity.AddEndOnTimeout(m_ConfigContext.GameConfig.value.LevelData.Timeout);
		}

		if (endingCondition.HasFlag(GameEndingCondition.Goal))
		{
			m_GameFlowContext.GameFlowEntity.AddEndOnGoalReached(m_ConfigContext.GameConfig.value.LevelData.GoalScore);
		}

		if (endingCondition.HasFlag(GameEndingCondition.Elimination))
		{
			m_GameFlowContext.GameFlowEntity.AddEndOnEliminated(m_ConfigContext.GameConfig.value.LevelData.NumberOfTeamsShouldBeLeft);
		}
	}
}
