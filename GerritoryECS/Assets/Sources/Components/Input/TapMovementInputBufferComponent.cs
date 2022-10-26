using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Input]
public sealed class TapMovementInputBufferComponent : IComponent
{
	public Movement.Type Type;
	public float DecayTimer;
}
