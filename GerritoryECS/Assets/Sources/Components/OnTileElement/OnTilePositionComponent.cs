using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
[System.Serializable]
public sealed class OnTilePositionComponent : IComponent
{
	[EntityIndex]
	public Vector2Int Value;
}
