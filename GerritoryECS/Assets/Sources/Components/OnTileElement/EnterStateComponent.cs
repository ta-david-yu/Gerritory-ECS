using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Element]
[Cleanup(CleanupMode.RemoveComponent)]
[Event(EventTarget.Self)]
public sealed class EnterStateComponent : IComponent
{
}
