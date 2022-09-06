using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemFeature : Feature
{
	public ItemFeature(Contexts contexts)
	{
		// Core item eating systems
		Add(new EatItemOnMoveEndSystem(contexts));

		// Apply item effect systems
		// ...

		// Generated cleanup systems
		Add(new RemoveEatenFromItemEntitiesSystem(contexts.Item));
	}
}
