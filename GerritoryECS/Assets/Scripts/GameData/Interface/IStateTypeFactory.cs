using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Type Factory stores all the information regarding states that are design-flexible (i.e. data oriented). 
/// Therefore not all the states in the game will be kept track of in this factory.
/// </summary>
public interface IStateTypeFactory
{
	public bool TryGetStateBlueprint(StateTypeEnum type, out IPlayerStateBlueprint blueprint);
	public bool TryGetStateName(StateTypeEnum type, out string name);
	public bool TryGetStateColor(StateTypeEnum tpe, out Color color);
}
