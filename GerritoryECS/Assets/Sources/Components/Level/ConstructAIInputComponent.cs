using JCMG.EntitasRedux;

[Level]
public sealed class ConstructAIInputComponent : IComponent
{
	public Movement.Type Movement;
	public int TargetPlayerId;
}