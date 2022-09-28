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
	private PlayerFactory m_PlayerFactory;
	public IPlayerFactory PlayerFactory => m_PlayerFactory;

	[SerializeField]
	private List<PlayerGameConfig> m_PlayerGameConfigs = new List<PlayerGameConfig>();
	public ReadOnlyCollection<PlayerGameConfig> PlayerGameConfigs => m_PlayerGameConfigs.AsReadOnly();

	[SerializeField]
	private LevelData m_LevelData;
	public LevelData LevelData => m_LevelData;

	[SerializeField]
	private GameObjective m_Objective = GameObjective.Score;
	public GameObjective Objective => m_Objective;

	[SerializeField]
	[EnumFlags]
	private GameEndingCondition m_EndingCondition = GameEndingCondition.Timeout;
	public GameEndingCondition EndingCondition => m_EndingCondition;

	[SerializeField]
	private float m_Timeout = 30;
	public float Timeout => m_Timeout;

	[SerializeField]
	private int m_GoalScore = 1;
	public int GoalScore => m_GoalScore;

	[SerializeField]
	private int m_NumberOfTeamsShouldBeLeft = 1;
	public int NumberOfTeamsShouldBeLeft => m_NumberOfTeamsShouldBeLeft;
}
