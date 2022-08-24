using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Game]
[Event(EventTarget.Self)]
public sealed class MoveOnTileComponent : IComponent
{
	// Move progress ranges from 0 to 1, where 1 means the move is completed
	public float Progress;
	public Vector2Int FromPosition;
	public Vector2Int ToPosition;
}
