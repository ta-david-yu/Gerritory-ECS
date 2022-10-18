using JCMG.EntitasRedux;

[Command]
public sealed class MarkOnTileElementDeadComponent : IComponent
{
	[EntityIndex]
	public int TargetOnTileElementId;
}
