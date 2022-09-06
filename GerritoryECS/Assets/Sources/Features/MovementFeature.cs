using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class MovementFeature : Feature
{
	public MovementFeature(Contexts contexts)
	{
		Add(new CommandMoveOnTileSystem(contexts));
		Add(new MoveOnTileSystem(contexts));

		// Generated cleanup systems
		Add(new RemoveMoveOnTileBeginFromGameEntitiesSystem(contexts.Game));
		Add(new RemoveMoveOnTileEndFromGameEntitiesSystem(contexts.Game));
	}
}
