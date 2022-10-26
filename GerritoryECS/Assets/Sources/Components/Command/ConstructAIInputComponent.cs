using JCMG.EntitasRedux;

[Command]
public sealed class ConstructAIInputComponent : IComponent
{
	public Movement.Type Movement;
	public int TargetElementId;
}