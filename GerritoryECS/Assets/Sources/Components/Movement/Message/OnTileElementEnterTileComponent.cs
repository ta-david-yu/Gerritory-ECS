using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Message]
public sealed class OnTileElementEnterTileComponent : IComponent
{
	[PrimaryEntityIndex]
	public int OnTileElementId;

	/// <summary>
	/// The position of the tile that the OnTileElement is entering.
	/// </summary>
	public Vector2Int Position;
}
