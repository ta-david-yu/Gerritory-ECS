using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Input]
[System.Serializable]
public sealed class UserInputComponent : IComponent
{
	/// <summary>
	/// The input device index
	/// </summary>
	[EntityIndex]
	public int Index;

	/// <summary>
	/// The player id this user input is controlling
	/// </summary>
	public int TargetPlayerId;
}
