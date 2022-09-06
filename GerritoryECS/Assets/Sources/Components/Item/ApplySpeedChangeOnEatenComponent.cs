using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
[System.Serializable]
public sealed class ApplySpeedChangeOnEatenComponent : IComponent
{
	public float SpeedMultiplier = 1;
}
