using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

[Element, Input, Config, Tile, Item, PlayerState]
[System.Serializable]
public class DebugMessageComponent : IComponent
{
	public string Message;
}
