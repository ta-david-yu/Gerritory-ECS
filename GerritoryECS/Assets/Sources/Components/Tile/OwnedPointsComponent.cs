using JCMG.EntitasRedux;

/// <summary>
/// Determine how many points should be added to the team score when owned.
/// </summary>
[Tile]
[System.Serializable]
public sealed class OwnedPointsComponent : IComponent
{
	public int Value = 1;
}
