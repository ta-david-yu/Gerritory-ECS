using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Input]
[System.Serializable]
public sealed class AIInputComponent : IComponent
{
	public Movement.Type Movement;

	/// <summary>
	/// The player id this user input is controlling
	/// </summary>
	public int TargetPlayerId;
}
