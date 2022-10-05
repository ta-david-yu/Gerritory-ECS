using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build tile entities on request.
/// </summary>
public sealed class ConstructTileSystem : IFixedUpdateSystem
{
	private readonly RequestContext m_RequestContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<RequestEntity> m_ConstructTileRequestGroup;

	public ConstructTileSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_RequestContext = contexts.Request;

		m_ConstructTileRequestGroup = m_RequestContext.GetGroup(RequestMatcher.ConstructTile);
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
