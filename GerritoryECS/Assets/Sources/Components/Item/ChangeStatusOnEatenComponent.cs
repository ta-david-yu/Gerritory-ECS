using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Item]
[System.Serializable]
public sealed class ChangeStateOnEatenComponent : IComponent
{
	/// <summary>
	/// The state entity blueprint that gets applied when 
	/// </summary>
	public PlayerStateBlueprint StateEntityBlueprint;
}
