using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
[Event(EventTarget.Self)]
[System.Serializable]
public sealed class TeamComponent : IComponent
{
	[EntityIndex]
	public int Id;
}
