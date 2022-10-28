using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class SpawnIntervalComponent : IComponent
{
	public float MinValue;
	public float MaxValue;

	public float GetNextSpawnInterval()
	{
		return UnityEngine.Random.Range(MinValue, MaxValue);
	}
}
