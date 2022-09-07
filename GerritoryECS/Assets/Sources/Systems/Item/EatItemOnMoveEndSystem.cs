using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EatItemOnMoveEndSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;

	private readonly IGroup<GameEntity> m_EaterMoveOnTileEndGroup;

	public EatItemOnMoveEndSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;

		m_EaterMoveOnTileEndGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.ItemEater, GameMatcher.MoveOnTileEnd));
	}

	public void FixedUpdate()
	{
		foreach (var eaterEntity in m_EaterMoveOnTileEndGroup)
		{
			Vector2Int enterPosition = eaterEntity.OnTileElement.Position;
			
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
			itemOnPosition.AddEaten(eaterEntity.ItemEater.Id);
		}
	}
}
