using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Level, Unique]
[ComponentName("OnTileElementIdCounter", "TileOwnerIdCounter", "StateHolderIdCounter", "ItemEaterIdCounter")]
public struct UniqueIdCounter
{
	public int Value;
}
