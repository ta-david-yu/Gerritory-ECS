using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
	TileTypeTable TileTypeTable { get; }
	LevelData LevelData { get; }
}
