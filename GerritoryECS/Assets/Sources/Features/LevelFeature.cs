using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelFeature : Feature
{
	public LevelFeature(Contexts contexts)
	{
		// Level construction systems
		Add(new ConstructLevelSystem(contexts));
	}
}
