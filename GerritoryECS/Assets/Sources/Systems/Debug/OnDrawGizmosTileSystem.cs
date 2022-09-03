using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosTileSystem : IUpdateSystem
{
	private readonly TileContext m_TileContext;
	private IGroup<TileEntity> m_TileGroup;

	private readonly Color[] m_TileOwnerColors = new Color[] { Color.yellow, Color.blue, Color.red, Color.cyan, Color.green };

	public OnDrawGizmosTileSystem(Contexts contexts)
	{
		m_TileContext = contexts.Tile;
		m_TileGroup = m_TileContext.GetGroup(TileMatcher.TilePosition);
	}


	public void Update()
	{
		foreach (var tileEntity in m_TileGroup.GetEntities())
		{
			if (!tileEntity.IsEnterable)
			{
				Gizmos.color = Color.black;
			}
			else
			{
				if (tileEntity.HasOwnable)
				{
					if (tileEntity.Ownable.HasOwner)
					{
						int ownerId = tileEntity.Ownable.OwnerId;
						Gizmos.color = m_TileOwnerColors[ownerId % m_TileOwnerColors.Length];
					}
					else
					{
						Gizmos.color = Color.white;
					}
				}
				else
				{
					Gizmos.color = Color.grey;
				}
			}

			Vector3 worldPos = GameConstants.TilePositionToWorldPosition(tileEntity.TilePosition.Value);
			Gizmos.DrawCube(worldPos, Vector3.one * 0.2f);
		}
	}
}
