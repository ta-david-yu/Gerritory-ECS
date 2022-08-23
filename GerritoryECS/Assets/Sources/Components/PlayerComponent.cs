using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
public sealed class PlayerComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}
