using JCMG.EntitasRedux;
using UnityEngine;

[Element]
[Cleanup(CleanupMode.RemoveComponent)]
[Event(EventTarget.Self)]
public sealed class MoveOnTileBeginComponent : IComponent
{
	public Vector2Int FromPosition;
	public Vector2Int ToPosition;
}
