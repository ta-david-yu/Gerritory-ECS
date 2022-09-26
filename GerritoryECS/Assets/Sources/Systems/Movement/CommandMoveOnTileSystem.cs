using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// <see cref="CommandMoveOnTileSystem"/> apply movement to entity on tile that has movement input action component if the move is valid.
/// </summary>
public class CommandMoveOnTileSystem : IFixedUpdateSystem
{
	private Contexts m_Contexts;
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<GameEntity> m_OnTileElementGroup;
	private readonly IGroup<GameEntity> m_PlayerGroup;

	public CommandMoveOnTileSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_OnTileElementGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.OnTilePosition, GameMatcher.MovementInputAction));
		m_PlayerGroup = m_GameContext.GetGroup(GameMatcher.Player);
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_OnTileElementGroup.GetEntities())
		{
			if (entity.HasMoveOnTile || entity.HasMoveOnTileEnd)
			{
				// The entity is already moving / ending its previous move. Ignore the input action.
				continue;
			}

			// Calculate next move position based on the movement input action
			Vector2Int moveOffset = Movement.TypeToOffset[(int)entity.MovementInputAction.Type];
			Vector2Int fromPosition = entity.OnTilePosition.Value;
			Vector2Int toPosition = fromPosition + moveOffset;

			bool isMoveableTo = m_Contexts.IsTileAtPositionMoveableTo(toPosition);
			if (!isMoveableTo)
			{
				continue;
			}

			bool isPositionOccipied = m_Contexts.IsTileAtPositionOccupied(toPosition);
			if (isPositionOccipied)
			{
				if (!entity.IsOnTileElementKiller)
				{
					// The entity is not a killer, therefore cannot kill the occupier.
					continue;
				}

				// If entity is OnTileElementKiller, could possibly kill the occupier to take over the position.
				// For now, we assume there will only be at most 1 occupier on a tile.
				List<GameEntity> onTileEntities = m_GameContext.GetEntitiesWithOnTilePosition(toPosition).ToList();
				if (onTileEntities.Count > 0)
				{
					GameEntity occupierEntity = onTileEntities.First();
					if (!occupierEntity.IsCanBeDead)
					{
						// The occupier cannot be dead.
						continue;
					}

					if (occupierEntity.HasMoveOnTile)
					{
						// The occupier is moving away (escaping) from the tile.
						continue;
					}

					if (!entity.CanStepOnVictim(occupierEntity))
					{
						continue;
					}

					// Kill the occupier!
					TryKillResult killResult = m_Contexts.TryKill(occupierEntity);
					if (!killResult.Success)
					{
						// The kill action is not successful.
						continue;
					}
				}

				List<GameEntity> movingInEntities = m_GameContext.GetEntitiesWithMoveOnTile(toPosition).ToList();
				if (movingInEntities.Count > 0)
				{
					GameEntity movingInEntity = movingInEntities.First();
					if (!movingInEntity.IsCanBeDead)
					{
						// The moving-in entity cannot be dead.
						continue;
					}

					if (!entity.CanStepOnVictim(movingInEntity))
					{
						continue;
					}

					// Kill the moving-in entity!
					TryKillResult killResult = m_Contexts.TryKill(movingInEntity);
					if (!killResult.Success)
					{
						// The kill action is not successful.
						continue;
					}
				}
			}

			// Consume movement input action.
			entity.RemoveMovementInputAction();
			entity.AddMoveOnTileBegin(fromPosition, toPosition);
			entity.AddMoveOnTile(0, fromPosition, toPosition);

			// Emit global LeaveTile message.
			m_MessageContext.EmitOnTileElementLeaveTileMessage(entity.OnTileElement.Id, fromPosition);
		}
	}
}
