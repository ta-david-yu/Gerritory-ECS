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

		// Update AI input idle timer
		Add(new UpdateIdleTimerSystem(contexts));

		// Read/emit input from Users or AIs
		Add(new IdleAIOnRespawnSystem(contexts));
		Add(new EmitUserInputSystem(contexts));
		Add(new EmitAIInputSystem(contexts));

		// Input Action Validation
		Add(new DecayMovementInputActionSystem(contexts));
	}
}
