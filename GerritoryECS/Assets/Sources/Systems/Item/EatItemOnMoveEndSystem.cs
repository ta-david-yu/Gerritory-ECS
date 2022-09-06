using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EatItemOnMoveEndSystem : IUpdateSystem
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

	public void Update()
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
				Debug.LogWarning($"Normally an item that is eaten shouldn't be still on the tile {enterPosition}. Something is wrong here!");
				continue;
			}

			// Eat the item!
			itemOnPosition.AddEaten(eaterEntity.ItemEater.Id);
		}
	}
}
