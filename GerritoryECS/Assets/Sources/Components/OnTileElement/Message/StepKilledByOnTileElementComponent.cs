using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Command, Message]
public sealed class StepKilledByOnTileElementComponent : IComponent
{
	[EntityIndex]
	public int KillerOnTileElementId;
}
