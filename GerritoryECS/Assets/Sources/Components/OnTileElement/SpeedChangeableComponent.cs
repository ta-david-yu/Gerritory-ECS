using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Element]
[System.Serializable]
public sealed class SpeedChangeableComponent : IComponent
{
	public float BaseSpeed = 1.0f;
	public float SpeedMultiplier = 1.0f;
}
