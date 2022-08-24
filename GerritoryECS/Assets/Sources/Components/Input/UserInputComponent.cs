using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Input]
[System.Serializable]
public sealed class UserInputComponent : IComponent
{
	/// <summary>
	/// The user input device index
	/// </summary>
	[EntityIndex]
	public int UserIndex;

	/// <summary>
	/// The player id this user input is controlling
	/// </summary>
	public int TargetPlayerId;
}
