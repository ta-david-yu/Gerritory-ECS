using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateWaitingForRespawnStateOnDeathSystem : ReactiveSystem<ElementEntity>
{
	private readonly ElementContext m_ElementContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly Contexts m_Contexts;

	public CreateWaitingForRespawnStateOnDeathSystem(Contexts contexts) : base(contexts.Element)
	{
		m_ElementContext = contexts.Element;
		m_PlayerStateContext = contexts.PlayerState;
		m_Contexts = contexts;
	}

	protected override ICollector<ElementEntity> GetTrigger(IContext<ElementEntity> context)
	{
		return context.CreateCollector(ElementMatcher.AllOf(ElementMatcher.OnTileElement, ElementMatcher.CanBeDead, ElementMatcher.Dead));
	}

	protected override bool Filter(ElementEntity entity)
	{
		return entity.IsDead && entity.IsCanRespawnAfterDeath;
	}

	protected override void Execute(List<ElementEntity> entities)
	{
		foreach (var deadEntity in entities)
		{
			if (!deadEntity.HasStateHolder)
			{
				// The dead entity cannot hold a state, skip it.
				continue;
			}

			int stateHolderId = deadEntity.StateHolder.Id;

			// Remove existing states targetting the holder because there should only be 1 state at a time.
			m_Contexts.RemovePlayerStateFor(stateHolderId);

			// Create a new state entity targetting the holder.
			PlayerStateEntity newStateEntity = m_Contexts.AddPlayerStateFor(stateHolderId);
			newStateEntity.AddTimer(GameConstants.WaitingForRespawnDuration);
			newStateEntity.AddWaitingForRespawnState(newRespawnAreaId: 0);	// TODO: add custom respawn area id here
		}
	}
}
