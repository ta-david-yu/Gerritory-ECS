using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class SpawnIntervalComponent : IComponent
{
	public float Value;

	public float GetNextSpawnInterval()
	{
		// TODO: randomized time value
		return Value;
	}
}
