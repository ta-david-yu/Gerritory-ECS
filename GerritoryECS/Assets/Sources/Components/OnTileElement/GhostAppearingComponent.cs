using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Element]
[Event(eventTarget: EventTarget.Self, eventType: JCMG.EntitasRedux.EventType.Added)]
public sealed class GhostAppearingComponent : IComponent
{
	public float Progress;
}
