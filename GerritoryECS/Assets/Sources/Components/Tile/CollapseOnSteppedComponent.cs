using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="CollapseOnSteppedComponent"/> keeps track of number of steps left until the tile become unenterable.
/// The counter decrements when an OnTileElement left the tile.
/// </summary>
[Game]
[System.Serializable]
public sealed class CollapseOnSteppedComponent : IComponent
{
	public int NumberOfStepsLeft;
}
