using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelFeature : Feature
{
	public LevelFeature(Contexts contexts)
	{
		// Init systems
		Add(new InitializeRandomSeedSystem(contexts));

		// Id counter initialize systems
		Add(new UniqueIdCounterSystem(contexts));

		// Level construction systems
		Add(new ConstructLevelSystem(contexts));

		// Player construction systems
		Add(new ConstructPlayerSystem(contexts));
	}
}
