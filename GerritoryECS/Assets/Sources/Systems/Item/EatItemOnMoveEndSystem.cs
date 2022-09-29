using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EatItemOnMoveEndSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_EnterTileMessageGroup;

	public EatItemOnMoveEndSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_MessageContext = contexts.Message;

		m_EnterTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.AllOf(MessageMatcher.OnTileElementEnterTile).NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity enterMessageEntity in m_EnterTileMessageGroup)
		{
			int onTileElementId = enterMessageEntity.OnTileElementEnterTile.OnTileElementId;
			ElementEntity entererEntity = m_ElementContext.GetEntityWithOnTileElement(onTileElementId);
			if (!entererEntity.HasItemEater)
			{
				// The entering OnTileElement is not an ItemEater, do nothing.
				continue;
			}

			Vector2Int enterPosition = enterMessageEntity.OnTileElementEnterTile.Position;
			ItemEntity itemOnPosition = m_ItemContext.GetEntityWithOnTileItem(enterPosition);
			if (itemOnPosition == null)
			{
				// There is no item at the enter position. Nothing eaten.
				continue;
			}

			if (itemOnPosition.HasEaten)
			{
				Debug.LogWarning($"Normally an item that is marked as eaten shouldn't be on the tile {enterPosition}. Something is wrong here!");
				continue;
			}

			// TODO: Check more logic if this item is actually edible to the item eater
			// ...

			// Mark this item as eaten,
			// it will later be processed by its respective reactive system to actually apply the item effect on the eater.
			itemOnPosition.AddEaten(entererEntity.ItemEater.Id);
		}
	}
}
