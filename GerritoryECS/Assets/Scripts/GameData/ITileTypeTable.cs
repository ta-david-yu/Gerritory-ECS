using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileTypeTable
{
	public ITileBlueprint GetTileBlueprint(string tileId);
	public IEntityCreationEventController CreateViewForTileEntity(string tileId, TileEntity tileEntity, Vector2Int tilePosition);
}
