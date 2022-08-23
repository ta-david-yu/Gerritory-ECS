using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Game]
[System.Serializable]
public sealed class TilePositionComponent : IComponent
{
	[EntityIndex]
	public Vector2Int Value;
}

