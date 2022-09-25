using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tile]
[Cleanup(CleanupMode.RemoveComponent)]
[Event(EventTarget.Self)]
public sealed class EnteredByOnTileElementComponent : IComponent
{
	public int OnTileElementId;
}
