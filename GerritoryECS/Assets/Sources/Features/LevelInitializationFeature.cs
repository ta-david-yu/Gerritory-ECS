using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InitializationFeature : Feature
{
	public InitializationFeature(Contexts contexts)
	{
		// Initialization systems
		{
			// Init random generator system
			Add(new InitializeRandomSeedSystem(contexts));

			// Id counter initialize systems
			Add(new InitializeUniqueIdCounterSystem(contexts));
		}
	}
}
