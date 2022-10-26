using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Input]
[System.Serializable]
public sealed class AIInputComponent : IComponent
{
	/// <summary>
	/// The OnTileElement id this ai input is controlling
	/// </summary>
	[PrimaryEntityIndex]
	public int TargetElementId;

	public AIHelper.SearchSimulationState SearchSimulationState;
}
