using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;
using System;

/// <summary>
/// Do take over logic if entities that enter a tile have <see cref="TileOwnerComponent"/> + the entering tile is <see cref="OwnableComponent"/>
/// </summary>
public sealed class TakeOverOwnableOnMoveEndSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_EnterTileMessageGroup;

	public TakeOverOwnableOnMoveEndSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		m_EnterTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.AllOf(MessageMatcher.OnTileElementEnterTile).NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity enterMessageEntity in m_EnterTileMessageGroup)
		{
			int onTileElementId = enterMessageEntity.OnTileElementEnterTile.OnTileElementId;
			GameEntity entererEntity = m_GameContext.GetEntityWithOnTileElement(onTileElementId);
			if (!entererEntity.HasTileOwner)
			{
				// The entering OnTileElement is not a TileOwner, do nothing.
				continue;
			}

			Vector2Int enterTilePosition = enterMessageEntity.OnTileElementEnterTile.Position;
			TileEntity enterTileEntity = m_TileContext.GetEntityWithTilePosition(enterTilePosition);
			if (!enterTileEntity.HasOwnable)
			{
				// The tile is not ownable, do nothing.
				continue;
			}

			// TODO: take-overable check logic
			// ...

			if (enterTileEntity.Ownable.HasOwner)
			{
				// Decrement previous owner's number of owned tiles.
				GameEntity previousOwnerEntity = m_GameContext.GetEntityWithTileOwner(enterTileEntity.Ownable.OwnerId);
				previousOwnerEntity.ReplaceTileOwner(previousOwnerEntity.TileOwner.Id, previousOwnerEntity.TileOwner.NumberOfOwnedTiles - 1);
			}

			// Take over the tile and increment owner's number of owned tiles.
			enterTileEntity.ReplaceOwnable(true, entererEntity.TileOwner.Id);
			entererEntity.ReplaceTileOwner(entererEntity.TileOwner.Id, entererEntity.TileOwner.NumberOfOwnedTiles + 1);
		}
	}
}
