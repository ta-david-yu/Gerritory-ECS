using JCMG.EntitasRedux;

/// <summary>
/// The target <see cref="StateHolder"/> with <see cref="OnTileElementComponent"/> will be respawned once this state is over.
/// </summary>
[PlayerState]
public sealed class WaitingForRespawnState : IComponent
{
	public int RespawnAreaId;
}
