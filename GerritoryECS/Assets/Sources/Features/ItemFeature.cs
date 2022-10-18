using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemFeature : Feature
{
	public ItemFeature(Contexts contexts)
	{
		// Core item interaction systems
		Add(new EatItemOnMoveEndSystem(contexts));
		Add(new UpdateStateTimerSystem(contexts));

		// Create powerup item / state types systems
		Add(new ApplyStateTypeToEaterOnItemEatenSystem(contexts));

		// Item spawning related systems
		Add(new TimedGlobalItemSpawnerSystem(contexts));
		Add(new SpawnItemSystem(contexts));
	}
}
