using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AddIgnoredByGhostOnRespawnSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_RespawnMessageGroup;

	public AddIgnoredByGhostOnRespawnSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		m_RespawnMessageGroup = m_MessageContext.GetGroup(MessageMatcher.
			AllOf(MessageMatcher.OnTileElementRespawn).
			NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity respawnMessageEntity in m_RespawnMessageGroup.GetEntities())
		{
			int onTileElementId = respawnMessageEntity.OnTileElementRespawn.OnTileElementId;
			ElementEntity respawnedEntity = m_ElementContext.GetEntityWithOnTileElement(onTileElementId);

			respawnedEntity.IsIgnoredByGhost = true;
			respawnedEntity.AddRemoveIgnoredByGhostTimer(GameConstants.IgnoredByGhostOnRespawnTime);
		}
	}
}
