using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Game]
[Event(EventTarget.Self)]
[System.Serializable]
public sealed class OnTileElementComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;

	[EntityIndex]
	public Vector2Int Position;
}