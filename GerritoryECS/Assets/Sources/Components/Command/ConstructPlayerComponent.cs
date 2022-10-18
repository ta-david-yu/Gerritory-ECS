using JCMG.EntitasRedux;

[Command]
public sealed class ConstructPlayerComponent : IComponent
{
	public int PlayerId;
	public string PlayerName;
	public int TeamId;
	public int SkinId;
}
