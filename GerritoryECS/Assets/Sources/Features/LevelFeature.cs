using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelFeature : Feature
{
	public LevelFeature(Contexts contexts)
	{
		// Initialization systems
		{
			// Init random generator system
			Add(new InitializeRandomSeedSystem(contexts));

			// Id counter initialize systems
			Add(new InitializeUniqueIdCounterSystem(contexts));

			// Setup game info related systems
			Add(new SetupGameInfoSystem(contexts));
			Add(new SetupGameObjectiveSystem(contexts));

			// Level construction systems
			Add(new ConstructLevelSystem(contexts));

			// Player construction systems
			Add(new ConstructPlayerSystem(contexts));
		}

		// FixedUpdate systems
		{
			// TODO: GameInfoUpdateSystems
		}
	}
}
