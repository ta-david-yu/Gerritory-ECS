using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game]
[System.Serializable]
public class StateHolderComponent : IComponent
{
	[PrimaryEntityIndex]
	public int Id;
}
