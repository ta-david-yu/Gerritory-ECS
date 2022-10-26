using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Game Config", fileName = "GameConfig")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
	[SerializeField]
	private int m_InitialRandomSeed;
	public int InitialRandomSeed => m_InitialRandomSeed;

	[SerializeField]
	private TileFactory m_TileFactory;
	public ITileFactory TileFactory => m_TileFactory;

	[SerializeField]
	private OnTileElementFactory m_OnTileElementFactory;
	public IOnTileElementFactory OnTileElementFactory => m_OnTileElementFactory;

	[SerializeField]
	private StateTypeFactory m_StateTypeFactory;
	public IStateTypeFactory StateTypeFactory => m_StateTypeFactory;

	[SerializeField]
	private GameInfoViewFactory m_GameInfoViewFactory;
	public IGameInfoViewFactory GameInfoViewFactory => m_GameInfoViewFactory;

	[SerializeField]
	private List<PlayerGameConfig> m_PlayerGameConfigs = new List<PlayerGameConfig>();
	public ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs => m_PlayerGameConfigs.AsReadOnly();

	[SerializeField]
	private LevelData m_LevelData;
	public LevelData LevelData => m_LevelData;

	[ContextMenu("Auto-assign PlayerIds in PlayerConfigs")]
	private void assignPlayerIdBasedOnArrayIndex()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Update PlayerIds in PlayerConfigs");
		for (int i = 0; i < m_PlayerGameConfigs.Count; i++)
		{
			var playerConfig = m_PlayerGameConfigs[i];
			playerConfig.PlayerId = i;
			m_PlayerGameConfigs[i] = playerConfig;
		}
#endif
	}
}
