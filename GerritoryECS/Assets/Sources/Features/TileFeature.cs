using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TileFeature : Feature
{
	public TileFeature(Contexts contexts)
	{
		// Construct tile systems
		Add(new ConstructTileSystem(contexts));

		// Tile systems
		Add(new TakeOverOwnableOnEnterTileSystem(contexts));
		Add(new CollapseTileOnLeaveTileSystem(contexts));

		// Tile related event component systems
		Add(new CreateEnteredByOnTileElementSystem(contexts));
		Add(new CreateLeftByOnTileElementSystem(contexts));
	}
}

