using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tile]
[System.Serializable]
public sealed class CanBeRespawnedOnComponent : IComponent
{
	[EntityIndex]
	public int RespawnAreaId;
}
