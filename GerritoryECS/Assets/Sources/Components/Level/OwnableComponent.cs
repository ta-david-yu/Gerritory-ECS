using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
public sealed class OwnableComponent : IComponent
{
	public int OwnerPlayerIndex;
}
