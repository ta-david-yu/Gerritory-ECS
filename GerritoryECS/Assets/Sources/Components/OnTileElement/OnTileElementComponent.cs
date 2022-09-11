using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Game]
[System.Serializable]
public sealed class OnTileElementComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}