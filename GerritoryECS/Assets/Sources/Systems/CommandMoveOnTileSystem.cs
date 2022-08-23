using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="CommandMoveOnTileSystem"/> apply movement to entity on tile that has movement input action component if the move is valid.
/// </summary>
public class CommandMoveOnTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly InputContext m_InputContext;

	private readonly IGroup<GameEntity> m_OnTileElementGroup;
	private readonly IGroup<InputEntity> m_PlayerInputGroup;

	public CommandMoveOnTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_InputContext = contexts.Input;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_OnTileElementGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MovementInputAction));
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_OnTileElementGroup.GetEntities())
		{
			// Calculate next move position based on the movement input action
			Vector2Int offset = Movement.TypeToOffset[(int)entity.MovementInputAction.Type];
			Vector2Int fromPosition = entity.OnTileElement.Position;
			Vector2Int toPosition = fromPosition + offset;

			// Consume movement input action
			entity.RemoveMovementInputAction();

			if (entity.HasMoveOnTile)
			{
				// The entity is already moving. Ignore the input action.
				continue;
			}

			// TODO: validate whether next tile position can be moved to
			bool isValidMove = true;
			if (!isValidMove)
			{
				continue;
			}

			entity.AddMoveOnTile(0, fromPosition, toPosition);

			Debug.Log($"MoveOnTile Added: {fromPosition} -> {toPosition}");
		}
	}
}
