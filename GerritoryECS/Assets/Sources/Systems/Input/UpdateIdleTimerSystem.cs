using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpdateIdleTimerSystem : IFixedUpdateSystem
{
	private readonly IGroup<InputEntity> m_IdleTimerGroup;

	public UpdateIdleTimerSystem(Contexts contexts)
	{
		m_IdleTimerGroup = contexts.Input.GetGroup(InputMatcher.IdleTimer);
	}

	public void FixedUpdate()
	{
		foreach (var timerEntity in m_IdleTimerGroup.GetEntities())
		{
			float timer = timerEntity.IdleTimer.Value;
			timer -= Time.fixedDeltaTime;

			if (timer <= 0)
			{
				timerEntity.RemoveIdleTimer();
			}
			else
			{
				timerEntity.ReplaceIdleTimer(timer);
			}
		}
	}
}
