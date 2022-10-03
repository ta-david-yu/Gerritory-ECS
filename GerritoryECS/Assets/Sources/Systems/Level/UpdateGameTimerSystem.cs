using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpdateGameTimerSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;

	private readonly IGroup<LevelEntity> m_GameTimerGroup;

	public UpdateGameTimerSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;

		m_GameTimerGroup = m_LevelContext.GetGroup(LevelMatcher.GameTimer);
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_GameTimerGroup.GetEntities())
		{
			entity.ReplaceGameTimer(entity.GameTimer.Value + Time.fixedDeltaTime);
		}
	}
}
