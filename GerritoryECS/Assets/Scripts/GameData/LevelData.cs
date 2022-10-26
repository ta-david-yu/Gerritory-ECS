using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "GameData/LevelData")]
public class LevelData : ScriptableObject
{
	[System.Serializable]
	public class TilePair
	{
		public Vector2Int Key;
		public TileData Value;
	}

	[Header("Tile Settings")]
	public List<TilePair> TileDataPairs = new List<TilePair>();

	public Vector2Int LevelBoundingRectSize;

	[Header("Objective Settings")]
	[SerializeField]
	private GameObjective m_Objective = GameObjective.Score;
	public GameObjective Objective => m_Objective;

	[SerializeField]
	[EnumFlags]
	private GameEndingCondition m_EndingCondition = GameEndingCondition.Timeout;
	public GameEndingCondition EndingCondition => m_EndingCondition;

	[SerializeField]
	private float m_Timeout = 30;
	/// <summary>
	/// The game ends when the game timer reaches <see cref="Timeout"/>.
	/// </summary>
	public float Timeout => m_Timeout;

	[SerializeField]
	private int m_GoalScore = 1;
	/// <summary>
	/// The game ends when a team reaches <see cref="GoalScore"/>.
	/// </summary>
	public int GoalScore => m_GoalScore;

	[SerializeField]
	private int m_NumberOfTeamsShouldBeLeft = 1;
	/// <summary>
	/// The game ends when <see cref="NumberOfTeamsShouldBeLeft"/> or less teams are left on the field.
	/// </summary>
	public int NumberOfTeamsShouldBeLeft => m_NumberOfTeamsShouldBeLeft;

	[Header("Item Spawning Settings")]
	public GlobalItemSpawnerConfig[] GlobalItemSpawnerConfigs = new GlobalItemSpawnerConfig[0];

	[Header("Debug Section")]
	[SerializeField]
	private int m_RandomSeed = 20;

	[SerializeField]
	private int m_Size = 10;

	[ContextMenu("Generate Level Randomly")]
	private void randomlyGenerateLevelTileData()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Generate Level Data Randomly");
		TileDataPairs.Clear();

		Vector2Int levelSize = new Vector2Int(m_Size, m_Size);
		Random.InitState(m_RandomSeed);

		for (int x = 0; x < levelSize.x; x++)
		{
			for (int y = 0; y < levelSize.y; y++)
			{
				Vector2Int position = new Vector2Int(x, y);
				string tileId = "normal";
				float random = Random.Range(0.0f, 1.0f);
				if (random > 1.0f)
				{
					tileId = "collapse";
				}
				else if (random > 0.2f)
				{
					tileId = "normal";
				}
				else
				{
					tileId = "respawn";
				}

				TileDataPairs.Add(new TilePair() { Key = position, Value = new TileData { TileId = tileId } });
			}
		}
#endif
	}

	[ContextMenu("Generate Maze Level Randomly")]
	private void randomlyGenerateMaze()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Generate Level Data Randomly");
		TileDataPairs.Clear();

		Vector2Int levelSize = new Vector2Int(m_Size, m_Size);
		Random.InitState(m_RandomSeed);

		Vector2Int mazeSize = levelSize / 2;

		for (int x = 0; x < mazeSize.x; x++)
		{
			for (int y = 0; y < mazeSize.y; y++)
			{
				Vector2Int position = new Vector2Int(x * 2, y * 2);
				string tileId = "normal";
				float random = Random.Range(0.0f, 1.0f);
				if (random > 1.0f)
				{
					tileId = "collapse";
				}
				else if (random > 0.2f)
				{
					tileId = "normal";
				}
				else
				{
					tileId = "respawn";
				}

				TileDataPairs.Add(new TilePair() { Key = position, Value = new TileData { TileId = tileId } });
			}
		}
#endif
	}

	[ContextMenu("Recalculate Level Bounding Rect Size")]
	private void recalculateBoundingRectSize()
	{
#if UNITY_EDITOR
		int xMax = -1;
		int yMax = -1;
		foreach (TilePair pair in TileDataPairs)
		{
			if (pair.Key.x > xMax)
			{
				xMax = pair.Key.x;
			}

			if (pair.Key.y > yMax)
			{
				yMax = pair.Key.y;
			}
		}

		UnityEditor.Undo.RecordObject(this, "Recalculate Level Bounding Rect Size");
		LevelBoundingRectSize = new Vector2Int(xMax + 1, yMax + 1);
#endif
	}
}
