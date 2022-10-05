using JCMG.EntitasRedux;

[Request]
public sealed class ConstructAIInputComponent : IComponent
{
	public Movement.Type Movement;
	public int TargetPlayerId;
}