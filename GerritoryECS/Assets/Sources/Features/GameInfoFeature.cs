using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class GameInfoFeature : Feature
{
	public GameInfoFeature(Contexts contexts)
	{
		Add(new SetupGameInfoSystem(contexts));
		Add(new UpdateGameInfoSystem(contexts));
		Add(new UpdateGameTimerSystem(contexts));
	}
}
