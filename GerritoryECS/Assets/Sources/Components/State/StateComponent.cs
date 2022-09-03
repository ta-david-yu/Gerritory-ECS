using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PlayerState]
[System.Serializable]
public class StateComponent : IComponent
{
	/// <summary>
	/// The id of the status holder that this status is targetting
	/// </summary>
	[EntityIndex]
	public int HolderId;
}
