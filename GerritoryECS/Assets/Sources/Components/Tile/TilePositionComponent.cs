using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Game]
[System.Serializable]
public sealed class TilePositionComponent : IComponent
{
	[PrimaryEntityIndex]
	public Vector2Int Value;
}

