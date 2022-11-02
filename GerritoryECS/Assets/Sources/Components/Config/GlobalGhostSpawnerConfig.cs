using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalGhostSpawnerConfig
{
	public enum MaximumInLevelGhostCountMethod
	{
		OneGhostLessThanNumberOfPlayers,
		SameAsTheNumberOfPlayers
	}

	public float InitialSpawnMinTime = 8.0f;
	public float InitialSpawnMaxTime = 13.0f;

	public float MinimumSpawnInterval = 5.0f;
	public float MaximumSpawnInterval = 8.0f;

	public MaximumInLevelGhostCountMethod GhostCountMethod = MaximumInLevelGhostCountMethod.OneGhostLessThanNumberOfPlayers;
	public int GhostCountHardLimit = 8;
}
