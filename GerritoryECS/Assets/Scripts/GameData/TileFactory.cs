using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileFactory", menuName = "GameData/TileFactory")]
public sealed class TileFactory : ScriptableObject, ITileFactory, ISerializationCallbackReceiver
{
	[System.Serializable]
	private class TileTypePair
	{
		public string Key;
		public TileType Value;
	}

	public Dictionary<string, TileType> TileTypes = new Dictionary<string, TileType>();

	[Tooltip("Tile type that will be used when an unrecognizable tile id is presented.")]
	[SerializeField]
	private TileType m_FallbackTileType;

	[SerializeField]
	private List<TileTypePair> m_TileTypePairs = new List<TileTypePair>();

	private GameObject m_CreatedViewRoot;
	public GameObject CreatedViewRoot
	{
		get
		{
			if (m_CreatedViewRoot == null)
			{
				m_CreatedViewRoot = new GameObject("TileViewsRoot");
			}

			return m_CreatedViewRoot;
		}
	}

	public ITileBlueprint GetTileBlueprint(string tileId)
	{
		if (!TileTypes.TryGetValue(tileId, out TileType tileType))
		{
			// Cannot find a tile type with the given tile id, use the fallback type.
			tileType = m_FallbackTileType;
		}

		return tileType.Blueprint;
	}

	public IEntityCreationEventController CreateTileView(string tileId)
	{
		if (!TileTypes.TryGetValue(tileId, out TileType tileType))
		{
			// Cannot find a tile type with the given tile id, use the fallback type.
			tileType = m_FallbackTileType;
		}

		var tileUnityView = GameObject.Instantiate(tileType.Prefab, CreatedViewRoot.transform);
		return tileUnityView;
	}

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
