using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Level]
public sealed class TeamInfoComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}
