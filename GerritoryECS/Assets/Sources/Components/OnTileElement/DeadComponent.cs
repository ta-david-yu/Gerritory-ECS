using JCMG.EntitasRedux;

[Element]
[Event(eventTarget: EventTarget.Self, eventType: EventType.Added)]
[Event(eventTarget: EventTarget.Self, eventType: EventType.Removed)]
public sealed class DeadComponent : IComponent
{
}
