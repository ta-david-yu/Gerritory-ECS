using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;
using System;

/// <summary>
/// Do take over logic if entities that end a movement have <see cref="TileOwnerComponent"/> + the move-to tile is <see cref="OwnableComponent"/>
/// </summary>
public sealed class TakeOverOwnableOnMoveEndSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;

	private readonly IGroup<GameEntity> m_EnteringOwnerGroup;

	public TakeOverOwnableOnMoveEndSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;

		m_EnteringOwnerGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.TileOwner, GameMatcher.MoveOnTileEnd));
	}

	public void FixedUpdate()
	{
		foreach (var ownerEntity in m_EnteringOwnerGroup.GetEntities())
		{
			Vector2Int enterTilePosition = ownerEntity.MoveOnTileEnd.ToPosition;
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
			enterTileEntity.ReplaceOwnable(true, ownerEntity.TileOwner.Id);
			ownerEntity.ReplaceTileOwner(ownerEntity.TileOwner.Id, ownerEntity.TileOwner.NumberOfOwnedTiles + 1);
		}
	}
}
