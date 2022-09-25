using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TileFeature : Feature
{
	public TileFeature(Contexts contexts)
	{
		// Tile systems
		Add(new TakeOverOwnableOnMoveEndSystem(contexts));
		Add(new CollapseTileOnLeaveTileSystem(contexts));

		// Tile related event component systems
		Add(new CreateEnteredByOnTileElementSystem(contexts));
		Add(new CreateLeftByOnTileElementSystem(contexts));
	}
}

