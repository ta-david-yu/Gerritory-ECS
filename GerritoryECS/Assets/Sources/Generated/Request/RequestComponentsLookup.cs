//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class RequestComponentsLookup
{
	public const int ConstructAIInput = 0;
	public const int ConstructPlayer = 1;
	public const int ConstructTile = 2;
	public const int ConstructUserInput = 3;

	public const int TotalComponents = 4;

	public static readonly string[] ComponentNames =
	{
		"ConstructAIInput",
		"ConstructPlayer",
		"ConstructTile",
		"ConstructUserInput"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ConstructAIInputComponent),
		typeof(ConstructPlayerComponent),
		typeof(ConstructTileComponent),
		typeof(ConstructUserInputComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ConstructAIInputComponent), 0 },
		{ typeof(ConstructPlayerComponent), 1 },
		{ typeof(ConstructTileComponent), 2 },
		{ typeof(ConstructUserInputComponent), 3 }
	};

	/// <summary>
	/// Returns a component index based on the passed <paramref name="component"/> type; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="component"></param>
	public static int GetComponentIndex(IComponent component)
	{
		return GetComponentIndex(component.GetType());
	}

	/// <summary>
	/// Returns a component index based on the passed <paramref name="componentType"/>; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="componentType"></param>
	public static int GetComponentIndex(Type componentType)
	{
		return ComponentTypeToIndex.TryGetValue(componentType, out var index) ? index : -1;
	}
}
