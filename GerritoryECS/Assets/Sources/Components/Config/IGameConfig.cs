using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
	public enum GameObjective
	{
		Survival,
		Goal
	}

	int InitialRandomSeed { get; }
	ITileFactory TileFactory { get; }
	IPlayerFactory PlayerFactory { get; }
	ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs { get; }

	LevelData LevelData { get; }
	GameObjective Objective { get; }
}
