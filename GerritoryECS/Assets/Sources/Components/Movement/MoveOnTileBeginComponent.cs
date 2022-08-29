using JCMG.EntitasRedux;
using UnityEngine;

[Game]
[Event(EventTarget.Self)]
public sealed class MoveOnTileBeginComponent : IComponent
{
	public Vector2Int FromPosition;
	public Vector2Int ToPosition;
}
