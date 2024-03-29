using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnDrawGizmosDebugFeature : Feature
{
	public OnDrawGizmosDebugFeature(Contexts contexts)
	{
		Add(new OnDrawGizmosItemSystem(contexts));
		Add(new OnDrawGizmosUserInputSystem(contexts));
	}
}
