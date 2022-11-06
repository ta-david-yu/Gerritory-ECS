using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DisappearGhostOnStepKillSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<MessageEntity> m_KillMessageGroup;

	public DisappearGhostOnStepKillSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_MessageContext = contexts.Message;

		m_Contexts = contexts;

		m_KillMessageGroup = m_MessageContext.GetGroup(MessageMatcher.AllOf(MessageMatcher.OnTileElementDie, MessageMatcher.StepKilledByOnTileElement).NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity stepKillMessageEntity in m_KillMessageGroup.GetEntities())
		{
			int killerElementId = stepKillMessageEntity.StepKilledByOnTileElement.KillerOnTileElementId;
			ElementEntity killerEntity = m_ElementContext.GetEntityWithOnTileElement(killerElementId);
			if (!killerEntity.IsGhost)
			{
				// The killer is not a ghost, skip it.
				continue;
			}

			if (!killerEntity.IsCanDisappearAfterStepKill)
			{
				// The ghost doesn't disappear after a step kill, skip it.
				continue;
			}

			if (killerEntity.HasMoveOnTile)
			{
				// If the ghost is moving, we delay the disappearing process until it enters the next tile.
				killerEntity.IsDelayGhostDisappearOnEnterTile = true;
			}
			else
			{
				m_Contexts.TryMakeGhostDisappear(killerEntity);
			}
		}
	}
}
