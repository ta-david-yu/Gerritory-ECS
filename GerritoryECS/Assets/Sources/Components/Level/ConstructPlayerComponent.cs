using JCMG.EntitasRedux;

[Level]
public sealed class ConstructPlayerComponent : IComponent
{
	public int PlayerId;
	public string PlayerName;
	public int TeamId;
	public int SkinId;

	// TODO: Moved to a different InputRequest component
	public bool IsAI = false;
}
