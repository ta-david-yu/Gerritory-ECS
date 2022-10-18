using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class GlobalItemSpawnerComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
	public IItemBlueprint[] ItemBlueprintPool;
}
