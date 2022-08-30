using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// <see cref="CommandMoveOnTileSystem"/> apply movement to entity on tile that has movement input action component if the move is valid.
/// </summary>
public class CommandMoveOnTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;

	private readonly IGroup<GameEntity> m_OnTileElementGroup;
	private readonly IGroup<GameEntity> m_PlayerGroup;

	public CommandMoveOnTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_OnTileElementGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MovementInputAction));
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
			Vector2Int fromPosition = entity.OnTileElement.Position;
			Vector2Int toPosition = fromPosition + moveOffset;

			Vector2Int levelSize = m_GameContext.Level.LevelSize;
			if (toPosition.x < 0 || toPosition.x >= levelSize.x || toPosition.y < 0 || toPosition.y >= levelSize.y)
			{
				// The give position is out of bounds of the level size, cannot move here.
				continue;
			}

			GameEntity tileEntity = m_GameContext.GetEntityWithTilePosition(toPosition);
			if (tileEntity == null)
			{
				// There is no tile here, cannot move here.
				continue;
			}

			if (!tileEntity.IsEnterable)
			{
				// The tile is not enterable, cannot move here.
				continue;
			}

			HashSet<GameEntity> onTileEntities = m_GameContext.GetEntitiesWithOnTileElement(toPosition);
			if (onTileEntities.Any(entity => !entity.HasMoveOnTile))
			{
				// There are already more than one entities on the given tile position & not moving away.
				continue;
			}

			HashSet<GameEntity> movingToTargetPositionEntities = m_GameContext.GetEntitiesWithMoveOnTile(toPosition);
			if (movingToTargetPositionEntities.Count > 0)
			{
				// This position has already been reserved by another MoveOnTile entity.
				continue;
			}

			// Consume movement input action
			entity.RemoveMovementInputAction();
			entity.AddMoveOnTileBegin(fromPosition, toPosition);
			entity.AddMoveOnTile(0, fromPosition, toPosition);
		}
	}
}
