using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemFeature : Feature
{
	public ItemFeature(Contexts contexts)
	{
		// Core item related systems
		Add(new EatItemOnMoveEndSystem(contexts));
		Add(new UpdateStateTimerSystem(contexts));

		// Apply item effect systems
		Add(new ApplySpeedChangeStateOnItemEatenSystem(contexts));

		// Generated cleanup systems
		Add(new RemoveEatenFromItemEntitiesSystem(contexts.Item));
	}
}
