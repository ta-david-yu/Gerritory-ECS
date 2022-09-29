using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class ElementFeature : Feature
{
	public ElementFeature(Contexts contexts)
	{
		Add(new ConstructPlayerSystem(contexts));
	}
}
