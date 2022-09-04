using JCMG.EntitasRedux;
using UnityEngine;

[Game]
[Cleanup(CleanupMode.RemoveComponent)]
[Event(EventTarget.Self)]
public sealed class MoveOnTileEndComponent : IComponent
{
	public Vector2Int FromPosition;
	public Vector2Int ToPosition;
}
