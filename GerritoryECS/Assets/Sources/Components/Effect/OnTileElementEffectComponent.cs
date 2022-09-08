using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Effect]
public sealed class OnTileElementEffectComponent : IComponent
{
	[EntityIndex]
	public int OnTileElementId;
}
