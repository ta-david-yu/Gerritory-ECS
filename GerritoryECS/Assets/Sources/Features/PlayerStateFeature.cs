using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStateFeature : Feature
{
	public PlayerStateFeature(Contexts contexts)
	{
		// Death related systems
		Add(new CreateWaitingForRespawnStateOnDeathSystem(contexts));

		// State effects systems
		Add(new SpeedChangeStateSystem(contexts));
		Add(new RespawnOnWaitingForRespawnStateRemovedSystem(contexts));
	}
}