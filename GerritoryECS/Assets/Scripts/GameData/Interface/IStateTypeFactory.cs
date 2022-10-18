using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Type Factory stores all the information regarding states that are design-flexible (i.e. data oriented). 
/// Therefore not all the states in the game will be kept track of in this factory.
/// </summary>
public interface IStateTypeFactory
{
	public bool TryGetStateBlueprint(int id, out IPlayerStateBlueprint blueprint);
	public bool TryGetStateName(int id, out string name);
	public bool TryGetStateColor(int id, out Color color);
}
