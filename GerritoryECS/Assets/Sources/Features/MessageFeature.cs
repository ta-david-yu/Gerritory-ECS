using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MessageFeature : Feature
{
	public MessageFeature(Contexts contexts)
	{
		Add(new ConsumeMessageSystem(contexts));
	}
}
