using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Element]
[System.Serializable]
public sealed class PlayerComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}
