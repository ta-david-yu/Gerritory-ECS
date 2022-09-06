using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelFeature : Feature
{
	public LevelFeature(Contexts contexts)
	{
		// Level & Tiles construction systems
		Add(new ConstructLevelSystem(contexts));

		// Tile systems
		Add(new TakeOverOwnableOnMoveEndSystem(contexts));
		Add(new CollapseTileOnMoveBeginSystem(contexts));
	}
}
