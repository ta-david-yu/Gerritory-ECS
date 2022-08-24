using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class InputFeature : Feature
{
	public InputFeature(Contexts contexts)
	{
		// Read/emit Input from Users or AI
		Add(new EmitUserInputSystem(contexts));
		Add(new EmitAIInputSystem(contexts));

		// Input Action Validation
		Add(new CommandMoveOnTileSystem(contexts));
	}
}
