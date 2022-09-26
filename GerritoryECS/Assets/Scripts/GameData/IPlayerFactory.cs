using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerFactory
{
	public IEntityCreationEventController CreatePlayerView(int playerId, int teamId, int skinId);
}
