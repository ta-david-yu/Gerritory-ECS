using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Message]
public sealed class OnTileElementDieComponent : IComponent
{
	[PrimaryEntityIndex]
	public int OnTileElementId;
}
