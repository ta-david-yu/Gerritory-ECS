using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileTypeTable", menuName = "GameData/TileTypeTable")]
public sealed class TileTypeTable : ScriptableObject, ISerializationCallbackReceiver
{
	[System.Serializable]
	private class TileTypePair
	{
		public string Key;
		public TileType Value;
	}

	public Dictionary<string, TileType> TileTypes = new Dictionary<string, TileType>();

	[Tooltip("Tile type that will be used when an unrecognizable tile id is presented.")]
	public TileType FallbackTileType;

	[SerializeField]
	private List<TileTypePair> m_TileTypePairs = new List<TileTypePair>();

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		int index = 0;
		foreach (var pair in m_TileTypePairs)
		{
			if (!TileTypes.ContainsKey(pair.Key))
			{
				TileTypes.Add(pair.Key, pair.Value);
			}
			/*
			else
			{
				Debug.LogWarning($"There are more than 1 tile type entries with the same key value '{pair.Key}'. The one at index {index} is ignored.");
			}*/
			index++;
		}
	}
}
