using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Game Config", fileName = "GameConfig")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
	[SerializeField]
	private TileTypeTable m_TileTypeTable;
	public TileTypeTable TileTypeTable => m_TileTypeTable;

	[SerializeField]
	private LevelData m_LevelData;
	public LevelData LevelData => m_LevelData;
}
