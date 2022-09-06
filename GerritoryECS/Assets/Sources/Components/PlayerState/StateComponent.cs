using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PlayerState]
[System.Serializable]
public sealed class StateComponent : IComponent
{
	/// <summary>
	/// The id of the state holder that this status is targetting
	/// </summary>
	[EntityIndex]
	public int HolderId;
}
