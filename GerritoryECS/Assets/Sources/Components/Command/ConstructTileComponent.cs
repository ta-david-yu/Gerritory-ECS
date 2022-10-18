using JCMG.EntitasRedux;
using UnityEngine;

[Command]
public sealed class ConstructTileComponent : IComponent
{
	public Vector2Int TilePosition;
	public TileData TileData;
}
