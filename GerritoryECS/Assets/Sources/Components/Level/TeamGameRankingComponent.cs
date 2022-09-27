using JCMG.EntitasRedux;

/// <summary>
/// The team ranking order in the game. With a number of 1 means the team has the hightest score right now.
/// </summary>
[Level]
public sealed class TeamGameRankingComponent : IComponent
{
	/// <summary>
	/// Game Ranking ranges from 1 ~ 4. The smaller the ranking, the better the team is in the game.
	/// </summary>
	public int Number = 1;
}
