using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
public sealed class ReservedTileForPlayerComponent : IComponent
{
	public Vector2Int TilePosition;
	public int PlayerId;
}
