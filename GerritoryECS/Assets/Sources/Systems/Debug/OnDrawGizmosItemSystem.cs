using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosItemSystem : IUpdateSystem
{
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private IGroup<TileEntity> m_TileGroup;
	private IGroup<ItemEntity> m_ItemGroup;

	private readonly Color[] m_TileOwnerColors = new Color[] { Color.yellow, Color.blue, Color.red, Color.cyan, Color.green };

	public OnDrawGizmosItemSystem(Contexts contexts)
	{
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_TileGroup = m_TileContext.GetGroup(TileMatcher.TilePosition);
		m_ItemGroup = m_ItemContext.GetGroup(ItemMatcher.OnTileItem);
	}


	public void Update()
	{
		// Draw items on tile
		foreach (var itemEntity in m_ItemGroup.GetEntities())
		{
			Vector3 worldPos = GameConstants.TilePositionToWorldPosition(itemEntity.OnTileItem.Position);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(worldPos + Vector3.up * 0.5f, 0.3f);
		}
	}
}
