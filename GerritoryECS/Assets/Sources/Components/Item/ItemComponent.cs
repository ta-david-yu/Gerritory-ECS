using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
[System.Serializable]
public sealed class OnTileItemComponent : IComponent
{
	[PrimaryEntityIndex]
	public Vector2Int Position;
}
