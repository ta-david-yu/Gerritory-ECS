using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

[Element]
[System.Serializable]
public sealed class OnTileElementComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}