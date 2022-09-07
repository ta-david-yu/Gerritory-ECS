using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
public sealed class EatenComponent : IComponent
{
	[EntityIndex]
	public int EaterId;
}
