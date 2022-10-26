using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An OnTileElement with this component can kill other mortal OnTileElement by stepping on them.
/// </summary>
[Element]
[System.Serializable]
public sealed class OnTileElementKillerComponent : IComponent
{
}
