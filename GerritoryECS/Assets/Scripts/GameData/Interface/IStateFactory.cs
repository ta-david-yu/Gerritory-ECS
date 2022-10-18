using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateFactory
{
	public bool TryGetStateBlueprint(int id, out IPlayerStateBlueprint blueprint);
	public bool TryGetStateName(int id, out string name);
	public bool TryGetStateColor(int id, out Color color);
}
