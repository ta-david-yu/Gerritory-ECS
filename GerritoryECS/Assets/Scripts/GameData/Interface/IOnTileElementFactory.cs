using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnTileElementFactory
{
	public IEntityCreationEventController CreatePlayerView(int playerId, int teamId, int skinId);
	public IEntityCreationEventController CreateGhostView();
}
