using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum GameEndingCondition
{
	// If none of the condition flags are toggled, default (timeout of 30 seconds) will be used.
	Timeout = (1 << 0),
	Goal = (1 << 1),
	Elimination = (1 << 2)
}
