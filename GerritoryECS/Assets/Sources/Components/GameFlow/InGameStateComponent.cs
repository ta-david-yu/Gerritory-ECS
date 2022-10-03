using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GameFlow]
public sealed class InGameStateComponent : IComponent
{
	public enum State
	{
		Intro,
		Countdown,
		Playing,
		Outro
	}

	public State Value;
}
