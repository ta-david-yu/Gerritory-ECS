using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="CanBeDeadComponent"/> can be attached to an <see cref="OnTileElementComponent"/> to indicates the element can die.
/// </summary>
[Element]
[System.Serializable]
public sealed class CanBeDeadComponent : IComponent
{
}
