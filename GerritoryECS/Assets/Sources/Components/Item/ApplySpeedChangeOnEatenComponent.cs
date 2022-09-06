using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
[System.Serializable]
public sealed class ApplySpeedChangeStateOnEatenComponent : IComponent
{
	public float Duration = 3;
	public float SpeedMultiplier = 1;
}
