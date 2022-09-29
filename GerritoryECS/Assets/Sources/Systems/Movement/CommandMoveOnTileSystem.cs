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
	private readonly ElementContext m_ElementContext;
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<ElementEntity> m_OnTileElementGroup;
	private readonly IGroup<ElementEntity> m_PlayerGroup;

	public CommandMoveOnTileSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_ElementContext = contexts.Element;
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_OnTileElementGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.OnTileElement, ElementMatcher.OnTilePosition, ElementMatcher.MovementInputAction));
		m_PlayerGroup = m_ElementContext.GetGroup(ElementMatcher.Player);
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
				List<ElementEntity> onTileEntities = m_ElementContext.GetEntitiesWithOnTilePosition(toPosition).ToList();
				if (onTileEntities.Count > 0)
				{
					ElementEntity occupierEntity = onTileEntities.First();
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

					if (!m_Contexts.CanStepOnVictim(entity, occupierEntity))
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

				List<ElementEntity> movingInEntities = m_ElementContext.GetEntitiesWithMoveOnTile(toPosition).ToList();
				if (movingInEntities.Count > 0)
				{
					ElementEntity movingInEntity = movingInEntities.First();
					if (!movingInEntity.IsCanBeDead)
					{
						// The moving-in entity cannot be dead.
						continue;
					}

					if (!m_Contexts.CanStepOnVictim(entity, movingInEntity))
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
