using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
	int InitialRandomSeed { get; }
	ITileFactory TileFactory { get; }
	IPlayerFactory PlayerFactory { get; }
	IStateTypeFactory StateTypeFactory { get; }
	IGameInfoViewFactory GameInfoViewFactory { get; }
	ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs { get; }

	LevelData LevelData { get; }
	GameObjective Objective { get; }
	GameEndingCondition EndingCondition { get; }

	// Ending Condtion Variables

	/// <summary>
	/// The game ends when the game timer reaches <see cref="Timeout"/>.
	/// </summary>
	public float Timeout { get; }

	/// <summary>
	/// The game ends when a team reaches <see cref="GoalScore"/>.
	/// </summary>
	public int GoalScore { get; }

	/// <summary>
	/// The game ends when <see cref="NumberOfTeamsShouldBeLeft"/> or less teams are left on the field.
	/// </summary>
	public int NumberOfTeamsShouldBeLeft { get; }
}
