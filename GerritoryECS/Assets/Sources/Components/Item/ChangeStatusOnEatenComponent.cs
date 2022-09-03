using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
[System.Serializable]
public class ChangeStateOnEatenComponent : IComponent
{
	/// <summary>
	/// The state entity blueprint
	/// </summary>
	public PlayerStateBlueprint StateEntityBlueprint;
}
