using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build tile entities on request.
/// </summary>
public sealed class ConstructTileSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ElementContext m_ElementContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<LevelEntity> m_ConstructTileRequestGroup;

	public ConstructTileSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_LevelContext = contexts.Level;
		m_ElementContext = contexts.Element;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_ConfigContext = contexts.Config;

		m_ConstructTileRequestGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructTile);
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
