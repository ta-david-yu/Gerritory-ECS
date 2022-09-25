using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add <see cref="LeftByOnTileElementComponent"/> to tiles that are left by OnTileElements, by observing <see cref="OnTileElementLeaveTile"/> message entity.
/// </summary>
public sealed class CreateLeftByOnTileElementSystem : IUpdateSystem
{
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_LeaveTileMessageGroup;

	public CreateLeftByOnTileElementSystem(Contexts contexts)
	{
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		// Here we don't really care if the message entity consumed in FixedUpdate because we do our logic on Update.
		m_LeaveTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.
			AllOf(MessageMatcher.OnTileElementLeaveTile));
	}

	public void Update()
	{
		foreach (MessageEntity leaveMessageEntity in m_LeaveTileMessageGroup.GetEntities())
		{
			Vector2Int leavePosition = leaveMessageEntity.OnTileElementLeaveTile.Position;
			TileEntity leaveTileEntity = m_TileContext.GetEntityWithTilePosition(leavePosition);
			leaveTileEntity.AddLeftByOnTileElement(leaveMessageEntity.OnTileElementLeaveTile.OnTileElementId);
		}
	}
}
