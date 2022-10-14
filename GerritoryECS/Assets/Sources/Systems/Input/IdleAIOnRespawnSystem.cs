using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class IdleAIOnRespawnSystem : ReactiveSystem<ElementEntity>
{
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly Contexts m_Contexts;

	private const float k_MinimumStayTimeOnRespawn = 0.5f;

	public IdleAIOnRespawnSystem(Contexts contexts) : base(contexts.Element)
	{
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_Contexts = contexts;
	}

	protected override ICollector<ElementEntity> GetTrigger(IContext<ElementEntity> context)
	{
		return context.CreateCollector(ElementMatcher.Dead.Removed());
	}

	protected override bool Filter(ElementEntity entity)
	{
		return !entity.IsDead;
	}

	protected override void Execute(List<ElementEntity> entities)
	{
		foreach (var respawnedEntity in entities)
		{
			if (!respawnedEntity.HasPlayer)
			{
				continue;
			}

			InputEntity aiInputEntity = m_InputContext.GetEntityWithAIInput(respawnedEntity.Player.Id);
			if (aiInputEntity == null)
			{
				continue;
			}

			aiInputEntity.AddIdleTimer(k_MinimumStayTimeOnRespawn);
		}
	}
}
