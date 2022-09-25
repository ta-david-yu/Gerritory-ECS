//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class RemoveEnteredByOnTileElementFromTileEntitiesSystem : ICleanupSystem
{
	private readonly IGroup<TileEntity> _group;
	private readonly List<TileEntity> _entities;

	public RemoveEnteredByOnTileElementFromTileEntitiesSystem(IContext<TileEntity> context)
	{
		_group = context.GetGroup(TileMatcher.EnteredByOnTileElement);
		_entities = new List<TileEntity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].RemoveEnteredByOnTileElement();
		}
	}
}
