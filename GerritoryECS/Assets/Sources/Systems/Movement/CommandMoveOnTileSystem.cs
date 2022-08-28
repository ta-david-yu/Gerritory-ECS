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

	private readonly IGroup<GameEntity> m_OnTileElementGroup;
	private readonly IGroup<InputEntity> m_PlayerInputGroup;

	public CommandMoveOnTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_OnTileElementGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MovementInputAction));
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_OnTileElementGroup.GetEntities())
		{
			if (entity.HasMoveOnTile || entity.HasMoveOnTileComplete)
			{
				float decayTimer = entity.MovementInputAction.DecayTimer;
				decayTimer -= Time.fixedDeltaTime;
				if (decayTimer <= 0)
				{
					// If the input action has decayed, remove it!
					entity.RemoveMovementInputAction();
				}
				else
				{
					entity.ReplaceMovementInputAction(entity.MovementInputAction.Type, decayTimer);
				}

				// The entity is already moving / ending its previous move. Ignore the input action.
				continue;
			}

			// Calculate next move position based on the movement input action
			Vector2Int offset = Movement.TypeToOffset[(int)entity.MovementInputAction.Type];
			Vector2Int fromPosition = entity.OnTileElement.Position;
			Vector2Int toPosition = fromPosition + offset;

			// TODO: validate whether next tile position can be moved to.
			// Occupied? Empty?
			bool isValidMove = true;
			if (!isValidMove)
			{
				float decayTimer = entity.MovementInputAction.DecayTimer;
				decayTimer -= Time.fixedDeltaTime;
				if (decayTimer <= 0)
				{
					// If the input action has decayed, remove it!
					entity.RemoveMovementInputAction();
				}
				else
				{
					entity.ReplaceMovementInputAction(entity.MovementInputAction.Type, decayTimer);
				}

				continue;
			}

			// Consume movement input action
			entity.RemoveMovementInputAction();
			entity.AddMoveOnTile(0, fromPosition, toPosition);
		}
	}
}
