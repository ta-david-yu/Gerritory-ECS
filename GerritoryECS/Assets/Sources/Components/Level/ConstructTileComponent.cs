using JCMG.EntitasRedux;
using UnityEngine;

[Level]
public sealed class ConstructTileComponent : IComponent
{
	public Vector2Int TilePosition;
	public TileData TileData;
}
