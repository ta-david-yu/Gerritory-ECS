using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public sealed class GlobalItemSpawnerConfig
{
	[SerializeField]
	private List<ItemBlueprint> m_ItemBlueprintPool = new List<ItemBlueprint>();
	public IItemBlueprint[] ItemBlueprintPool => m_ItemBlueprintPool.ToArray();

	public float MinimumSpawnInterval = 5.0f;
	public float MaximumSpawnInterval = 5.0f;
	public int MaxNumberOfItemsOnLevelAtTheSameTime = 4;
}
