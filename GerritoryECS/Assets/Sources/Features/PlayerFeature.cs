using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class PlayerFeature : Feature
{
	public PlayerFeature(Contexts contexts)
	{
		Add(new ConstructPlayerSystem(contexts));
	}
}
