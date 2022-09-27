using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tile]
[Event(EventTarget.Self)]
[System.Serializable]
public sealed class OwnableComponent : IComponent
{
	public bool HasOwner;
	public int OwnerTeamId;
}
