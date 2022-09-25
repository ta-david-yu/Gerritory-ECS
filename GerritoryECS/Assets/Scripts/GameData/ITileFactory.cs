using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileFactory
{
	public ITileBlueprint GetTileBlueprint(string tileId);
	public IEntityCreationEventController CreateTileView(string tileId);
}
