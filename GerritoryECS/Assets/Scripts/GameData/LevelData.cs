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

	public List<TilePair> TileDataPairs = new List<TilePair>();

	public Vector2Int LevelBoundingRectSize;

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
				else if (random > 0.1f)
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
