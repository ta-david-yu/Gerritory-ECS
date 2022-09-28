using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Level, Unique]
public sealed class GameInfoComponent : IComponent
{
	public int CurrentHighestTeamScore;
	public int CurrentHighestTeamMemberCount;
}
