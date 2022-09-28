using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;
using System;

/// <summary>
/// Do take over logic if entities that enter a tile have <see cref="TileOwnerComponent"/> + the entering tile is <see cref="OwnableComponent"/>
/// </summary>
public sealed class TakeOverOwnableOnEnterTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly LevelContext m_LevelContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_EnterTileMessageGroup;

	public TakeOverOwnableOnEnterTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_LevelContext = contexts.Level;
		m_MessageContext = contexts.Message;

		m_EnterTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.AllOf(MessageMatcher.OnTileElementEnterTile).NoneOf(MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity enterMessageEntity in m_EnterTileMessageGroup)
		{
			int onTileElementId = enterMessageEntity.OnTileElementEnterTile.OnTileElementId;
			GameEntity entererEntity = m_GameContext.GetEntityWithOnTileElement(onTileElementId);
			if (!entererEntity.IsTileOwner)
			{
				// The entering OnTileElement is not a TileOwner, do nothing.
				continue;
			}

			if (!entererEntity.HasTeam)
			{
				// The entering OnTileElement is not in a team, do nothing.
				continue;
			}

			Vector2Int enterTilePosition = enterMessageEntity.OnTileElementEnterTile.Position;
			TileEntity enterTileEntity = m_TileContext.GetEntityWithTilePosition(enterTilePosition);
			if (!enterTileEntity.HasOwnable)
			{
				// The tile is not ownable, do nothing.
				continue;
			}

			if (enterTileEntity.HasOwner && enterTileEntity.Owner.OwnerTeamId == entererEntity.Team.Id)
			{
				// The tile has already been occupied by the team, do nothing.
				continue;
			}

			// TODO: take-overable check logic
			// ...

			int tileWorthPoints = enterTileEntity.Ownable.WorthPoints;

			if (enterTileEntity.HasOwner) 
			{
				// Decrement previous owner team number of owned tiles.
				int previousTeamId = enterTileEntity.Owner.OwnerTeamId;
				LevelEntity previousTeamEntity = m_LevelContext.GetEntityWithTeamInfo(previousTeamId);
				previousTeamEntity.ReplaceTeamScore(previousTeamEntity.TeamScore.Value - tileWorthPoints);
			}

			// Take over the tile.
			int newTeamId = entererEntity.Team.Id;
			enterTileEntity.ReplaceOwner(newTeamId);

			// Increment new owner team's number of owned tiles.
			LevelEntity newTeamEntity = m_LevelContext.GetEntityWithTeamInfo(newTeamId);
			newTeamEntity.ReplaceTeamScore(newTeamEntity.TeamScore.Value + tileWorthPoints);
		}
	}
}
