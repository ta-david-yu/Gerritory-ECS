using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DecayMovementInputActionSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly IGroup<GameEntity> m_InputActionGroup;

	public DecayMovementInputActionSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;

		// Get entities that
		// 1. Are on tile
		// 2. Have movement input action
		m_InputActionGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MovementInputAction));
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_InputActionGroup.GetEntities())
		{
			float decayTimer = entity.MovementInputAction.DecayTimer;

			// The reason why we don't guard == 0 is because, we want the MovementInputAction to exist at least for one frame before it's decayed.
			if (decayTimer < 0)
			{
				// If the input action has decayed, remove it!
				entity.RemoveMovementInputAction();
			}
			else
			{
				// Decrease decay timer.
				decayTimer -= Time.fixedDeltaTime;
				entity.ReplaceMovementInputAction(entity.MovementInputAction.Type, decayTimer);
			}
		}
	}
}
