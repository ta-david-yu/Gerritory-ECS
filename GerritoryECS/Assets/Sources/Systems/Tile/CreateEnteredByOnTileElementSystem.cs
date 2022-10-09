using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add <see cref="EnteredByOnTileElementComponent"/> to tiles that are entered by OnTileElements, by observing <see cref="OnTileElementEnterTile"/> message entity.
/// </summary>
public sealed class CreateEnteredByOnTileElementSystem : IUpdateSystem
{
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_EnterTileMessageGroup;

	public CreateEnteredByOnTileElementSystem(Contexts contexts)
	{
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		// Here we don't really care if the message entity consumed in FixedUpdate because we do our logic on Update.
		m_EnterTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.
			AllOf(MessageMatcher.OnTileElementEnterTile));
	}

	public void Update()
	{
		foreach (MessageEntity enterMessageEntity in m_EnterTileMessageGroup.GetEntities())
		{
			Vector2Int enterPosition = enterMessageEntity.OnTileElementEnterTile.Position;
			TileEntity enterTileEntity = m_TileContext.GetEntityWithTilePosition(enterPosition);

			if (enterTileEntity.HasEnteredByOnTileElement)
			{
				Debug.LogError
				(
					$"There has already been an EnteredBy component {enterTileEntity.EnteredByOnTileElement.OnTileElementId} on the tile.\n" +
					$"The new component is held by {enterMessageEntity.OnTileElementEnterTile.OnTileElementId}."
				);
				enterTileEntity.ReplaceEnteredByOnTileElement(enterMessageEntity.OnTileElementEnterTile.OnTileElementId);
			}
			else
			{
				enterTileEntity.AddEnteredByOnTileElement(enterMessageEntity.OnTileElementEnterTile.OnTileElementId);
			}
		}
	}
}
