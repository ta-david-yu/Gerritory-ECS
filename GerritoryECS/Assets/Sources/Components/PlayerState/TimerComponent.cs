using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PlayerState]
[Event(EventTarget.Self)]
public sealed class TimerComponent : IComponent
{
	public float Value;
}
