using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disable respawn
/// </summary>
public sealed class DisableRespawnForPlayerInEliminationConditionSystem : ReactiveSystem<GameFlowEntity>
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly ElementContext m_ElementContext;

	private readonly IGroup<ElementEntity> m_PlayerElementGroup;

	public DisableRespawnForPlayerInEliminationConditionSystem(Contexts contexts) : base(contexts.GameFlow)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_ElementContext = contexts.Element;

		m_PlayerElementGroup = m_ElementContext.GetGroup(ElementMatcher.Player);
	}
	protected override ICollector<GameFlowEntity> GetTrigger(IContext<GameFlowEntity> context)
	{
		return context.CreateCollector(GameFlowMatcher.EndOnEliminated);
	}

	protected override bool Filter(GameFlowEntity entity)
	{
		return entity.HasEndOnEliminated;
	}

	protected override void Execute(List<GameFlowEntity> entities)
	{
		foreach (var playerEntity in m_PlayerElementGroup.GetEntities())
		{
			// Disable respawn component for all the players.
			if (!playerEntity.IsCanRespawnAfterDeath)
			{
				continue;
			}

			playerEntity.IsCanRespawnAfterDeath = false;
		}
	}
}
