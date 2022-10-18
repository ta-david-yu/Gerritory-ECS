using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Command]
public sealed class SpawnItemComponent : IComponent
{
	[EntityIndex]
	public Vector2Int TilePosition;
	public IItemBlueprint ItemBlueprint;
}
