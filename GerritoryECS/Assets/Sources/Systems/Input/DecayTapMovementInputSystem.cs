using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayTapMovementInputSystem : IUpdateSystem
{
	private readonly InputContext m_InputContext;
	private readonly IGroup<InputEntity> m_InputEntityGroup;

	public DecayTapMovementInputSystem(Contexts contexts)
	{
		m_InputContext = contexts.Input;

		m_InputEntityGroup = m_InputContext.GetGroup(InputMatcher.TapMovementInputBuffer);
	}

	public void Update()
	{
		foreach (var entity in m_InputEntityGroup.GetEntities())
		{
			float decayTimer = entity.TapMovementInputBuffer.DecayTimer;

			// The reason why we don't guard == 0 is because, we want the MovementInputAction to exist at least for one frame before it's decayed.
			if (decayTimer < 0)
			{
				// If the input action has decayed, remove it!
				entity.RemoveTapMovementInputBuffer();
			}
			else
			{
				// Decrease decay timer.
				decayTimer -= Time.deltaTime;
				entity.ReplaceTapMovementInputBuffer(entity.TapMovementInputBuffer.Type, decayTimer);
			}
		}
	}
}
