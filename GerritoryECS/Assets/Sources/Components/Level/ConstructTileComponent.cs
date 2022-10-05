using JCMG.EntitasRedux;
using UnityEngine;

[Request]
public sealed class ConstructTileComponent : IComponent
{
	public Vector2Int TilePosition;
	public TileData TileData;
}
