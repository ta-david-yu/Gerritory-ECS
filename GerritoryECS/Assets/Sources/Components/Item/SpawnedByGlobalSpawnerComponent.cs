using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class SpawnedByGlobalSpawnerComponent : IComponent
{
	[EntityIndex]
	public int SpawnerId;
}
