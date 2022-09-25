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

	[ContextMenu("Generate Level Randomly")]
	private void randomlyGenerateLevelTileData()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Generate Level Data Randomly");
		TileDataPairs.Clear();

		Vector2Int levelSize = new Vector2Int(10, 10);
		Random.InitState(20);

		for (int x = 0; x < levelSize.x; x++)
		{
			for (int y = 0; y < levelSize.y; y++)
			{
				Vector2Int position = new Vector2Int(x, y);
				string tileId = "normal";
				float random = Random.Range(0.0f, 1.0f);
				if (random > 0.9f)
				{
					tileId = "collapse";
				}
				else if (random > 0.3f)
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
}
