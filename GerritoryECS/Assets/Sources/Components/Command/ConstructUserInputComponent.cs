using JCMG.EntitasRedux;

[Command]
public sealed class ConstructUserInputComponent : IComponent
{
	public int UserId;
	public int TargetElementId;
}
