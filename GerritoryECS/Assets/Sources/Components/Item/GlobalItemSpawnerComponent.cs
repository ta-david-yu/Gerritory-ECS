using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class GlobalItemSpawnerComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
	public IItemData[] ItemDataPool;

	public IItemData GetRandomItemFromPool()
	{
		int index = UnityEngine.Random.Range(0, ItemDataPool.Length);
		return ItemDataPool[index];
	}
}
