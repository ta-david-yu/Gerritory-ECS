using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class MovementDebugFeature : Feature
{
	public MovementDebugFeature(Contexts contexts)
	{
		Add(new OnGUIPlayerMovementSystem(contexts));
	}
}
