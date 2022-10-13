using JCMG.EntitasRedux;

[Level]
[Event(EventTarget.Self)]
public sealed class TeamScoreComponent : IComponent
{
	public int Value;
}
