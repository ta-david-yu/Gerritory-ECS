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
	IOnTileElementFactory OnTileElementFactory { get; }
	IStateTypeFactory StateTypeFactory { get; }
	IGameInfoViewFactory GameInfoViewFactory { get; }
	ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs { get; }

	LevelData LevelData { get; }
}
