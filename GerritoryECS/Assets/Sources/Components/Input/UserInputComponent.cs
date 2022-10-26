using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;
using UnityEngine.InputSystem;

[Input]
[System.Serializable]
public sealed class UserInputComponent : IComponent
{
	/// <summary>
	/// The user input device index
	/// </summary>
	[EntityIndex]
	public int UserId;

	/// <summary>
	/// The player id this user input is controlling
	/// </summary>
	[PrimaryEntityIndex]
	public int TargetPlayerId;
}
