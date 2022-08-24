using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class OnGUIDebugFeature : Feature
{
	public OnGUIDebugFeature(Contexts contexts)
	{
		Add(new OnGUIPlayerMovementSystem(contexts));
	}
}
