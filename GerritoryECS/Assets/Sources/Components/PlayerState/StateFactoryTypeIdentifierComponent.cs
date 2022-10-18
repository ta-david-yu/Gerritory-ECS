using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as the foreign key for UI to look up the color for the state timer UI. The identifier is basically the index of the StateFactory.StateTypes array
/// </summary>
[PlayerState]
public class StateFactoryTypeIdentifierComponent : IComponent
{
	public int Value;
}
