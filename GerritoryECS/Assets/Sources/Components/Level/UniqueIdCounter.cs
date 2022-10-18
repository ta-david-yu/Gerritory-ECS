using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Level, Unique]
[ComponentName("OnTileElementIdCounter", "StateHolderIdCounter", "ItemEaterIdCounter", "ItemSpawnerIdCounter")]
public struct UniqueIdCounter
{
	public int Value;
}
