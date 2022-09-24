using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Message]
public sealed class OnTileElementLeaveTileComponent : IComponent
{
	public int OnTileElementId;
	
	/// <summary>
	/// The position of the tile that the OnTileElement is leaving from.
	/// </summary>
	public Vector2Int Position;
}
