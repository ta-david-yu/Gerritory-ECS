using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
[System.Serializable]
public sealed class OwnableComponent : IComponent
{
	public bool HasOwner;
	public int OwnerId;
}
