using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build tile entities on request.
/// </summary>
public sealed class ConstructTileSystem : IFixedUpdateSystem
{
	private readonly CommandContext m_CommandContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<CommandEntity> m_ConstructTileRequestGroup;

	public ConstructTileSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_CommandContext = contexts.Command;

		m_ConstructTileRequestGroup = m_CommandContext.GetGroup(CommandMatcher.ConstructTile);
	}

	public void FixedUpdate()
	{
		foreach (var constructTileRequest in m_ConstructTileRequestGroup.GetEntities())
		{
			ConstructTileComponent constructTileComponent = constructTileRequest.ConstructTile;
			var tileEntity = m_Contexts.ConstructTileEntityAtPosition(constructTileComponent.TileData, constructTileComponent.TilePosition);

			constructTileRequest.Destroy();
		}
	}
}
