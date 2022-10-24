using JCMG.EntitasRedux;

[Level]
[Event(eventType: EventType.Added, eventTarget: EventTarget.Self)]
public sealed class LeadingTeamComponent : IComponent
{
	public int TeamId;
}
