using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class ElementFeature : Feature
{
	public ElementFeature(Contexts contexts)
	{
		// Entity construction systems.
		Add(new ConstructPlayerSystem(contexts));

		// Ghost related systems.
		Add(new GhostAppearingSystem(contexts));
		Add(new GhostDisappearingSystem(contexts));

		// Death / kill systems.
		Add(new MarkOnTileElementDeadSystem(contexts));
	}
}
