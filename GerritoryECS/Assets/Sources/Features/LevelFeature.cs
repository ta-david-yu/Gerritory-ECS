using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelFeature : Feature
{
	public LevelFeature(Contexts contexts)
	{
		Add(new ConstructLevelSystem(contexts));
		Add(new TakeOverOwnableOnMoveEndSystem(contexts));
	}
}
