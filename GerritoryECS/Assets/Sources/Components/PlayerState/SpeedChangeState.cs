using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PlayerState]
public sealed class SpeedChangeState : IComponent
{
	public float SpeedMultiplier = 1.0f;
}
