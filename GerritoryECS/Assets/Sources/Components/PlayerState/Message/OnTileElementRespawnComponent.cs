using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Message]
public sealed class OnTileElementRespawnComponent : IComponent
{
	[PrimaryEntityIndex]
	public int OnTileElementId;

	/// <summary>
	/// The position of the tile that the OnTileElement is respawning at.
	/// </summary>
	public Vector2Int RespawnPosition;
}
