using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Force set the priority of this event to -1, so LeaveState always happens before EnterState
/// </summary>
[Element]
[Cleanup(CleanupMode.RemoveComponent)]
[Event(EventTarget.Self, JCMG.EntitasRedux.EventType.Added, priority: -1)] 
public sealed class LeaveStateComponent : IComponent
{
}