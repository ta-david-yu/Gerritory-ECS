using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Game Config", fileName = "GameConfig")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
	[SerializeField]
	private TileTypeTable m_TileTypeTable;
	public ITileTypeTable TileTypeTable => m_TileTypeTable;

	[SerializeField]
	private LevelData m_LevelData;
	public LevelData LevelData => m_LevelData;

	[SerializeField]
	private List<PlayerGameConfig> m_PlayerGameConfigs = new List<PlayerGameConfig>();
	public ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs => m_PlayerGameConfigs.AsReadOnly();
}
