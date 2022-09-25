using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Game, Unique]
[System.Serializable]
public sealed class LevelComponent : IComponent
{
	public LevelData LevelData;
}
