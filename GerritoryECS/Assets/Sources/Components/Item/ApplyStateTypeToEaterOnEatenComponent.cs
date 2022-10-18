using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

[System.Serializable]
[Item]
public sealed class ApplyStateTypeToEaterOnEatenComponent : IComponent
{
	public StateTypeEnum StateType;
}
