using JCMG.EntitasRedux;

/// <summary>
/// If the entity is owned by a team.
/// </summary>
[Tile]
[Event(EventTarget.Self, EventType.Added)]
[Event(EventTarget.Self, EventType.Removed)]
public sealed class OwnerComponent : IComponent
{
	[EntityIndex]
	public int OwnerTeamId;
}
