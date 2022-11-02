using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpdateIgnoredByGhostTimerSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ElementEntity> m_IgnoredByGhostGroup;

	public UpdateIgnoredByGhostTimerSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_PlayerStateContext = contexts.PlayerState;
		m_Contexts = contexts;

		m_IgnoredByGhostGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.IgnoredByGhost, ElementMatcher.RemoveIgnoredByGhostTimer));
	}

	public void FixedUpdate()
	{
		foreach (var elementEntity in m_IgnoredByGhostGroup.GetEntities())
		{
			float timer = elementEntity.RemoveIgnoredByGhostTimer.Value;
			if (timer < 0)
			{
				elementEntity.IsIgnoredByGhost = false;
				elementEntity.RemoveRemoveIgnoredByGhostTimer();
			}
			else
			{
				timer -= Time.fixedDeltaTime;
				elementEntity.ReplaceRemoveIgnoredByGhostTimer(timer);
			}
		}
	}
}
