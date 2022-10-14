using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Input]
[System.Serializable]
public sealed class AIInputComponent : IComponent
{
	/// <summary>
	/// The player id this user input is controlling
	/// </summary>
	[PrimaryEntityIndex]
	public int TargetPlayerId;

	public AIHelper.SearchSimulationState SearchSimulationState;
}
