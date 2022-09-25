using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add <see cref="EnteredByOnTileElementComponent"/> to tiles that are entered by OnTileElements, by observing <see cref="OnTileElementEnterTile"/> message entity.
/// </summary>
public sealed class CreateEnteredByOnTileElementSystem : IFixedUpdateSystem
{
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_EnterTileMessageGroup;

	public CreateEnteredByOnTileElementSystem(Contexts contexts)
	{
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		m_EnterTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.
			AllOf(MessageMatcher.OnTileElementEnterTile).
			NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity enterMessageEntity in m_EnterTileMessageGroup.GetEntities())
		{
			Vector2Int enterPosition = enterMessageEntity.OnTileElementEnterTile.Position;
			TileEntity enterTileEntity = m_TileContext.GetEntityWithTilePosition(enterPosition);
			enterTileEntity.AddEnteredByOnTileElement(enterMessageEntity.OnTileElementEnterTile.OnTileElementId);
		}
	}
}
