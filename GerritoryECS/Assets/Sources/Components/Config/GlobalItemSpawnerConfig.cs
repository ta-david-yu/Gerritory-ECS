using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public sealed class GlobalItemSpawnerConfig
{
	[SerializeField]
	private List<ItemData> m_ItemDataPool = new List<ItemData>();
	public IItemData[] ItemDataPool => m_ItemDataPool.ToArray();

	public float MinimumSpawnInterval = 5.0f;
	public float MaximumSpawnInterval = 5.0f;
	public int MaxNumberOfItemsOnLevelAtTheSameTime = 4;
}
