using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tile]
[Event(EventTarget.Self, JCMG.EntitasRedux.EventType.Removed)]
[Event(EventTarget.Self, JCMG.EntitasRedux.EventType.Added)]
[System.Serializable]
public sealed class EnterableComponent : IComponent
{
}
