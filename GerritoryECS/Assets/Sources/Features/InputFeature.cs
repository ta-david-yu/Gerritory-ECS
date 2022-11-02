using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class InputFeature : Feature
{
	public InputFeature(Contexts contexts)
	{
		// Construct input systems
		Add(new ConstructInputEntitySystem(contexts));

		// Update AI input related timers
		Add(new UpdateIdleTimerSystem(contexts));
		Add(new UpdateIgnoredByGhostTimerSystem(contexts));

		// AI human-like staggering behaviour systems
		Add(new IdleAIOnRespawnSystem(contexts));
		Add(new AddIgnoredByGhostOnRespawnSystem(contexts));

		// Read/emit input from Users or AIs
		Add(new EmitUserInputSystem(contexts));
		Add(new EmitAIInputSystem(contexts));
		Add(new EmitChaseNearestOnTileElementVictimInputSystem(contexts));

		// Input Action Validation
		Add(new DecayMovementInputActionSystem(contexts));
	}
}
