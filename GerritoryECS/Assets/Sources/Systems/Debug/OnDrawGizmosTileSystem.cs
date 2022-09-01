using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosTileSystem : IUpdateSystem
{
	private readonly GameContext m_GameContext;
	private IGroup<GameEntity> m_TileGroup;

	private readonly Color[] m_TileOwnerColors = new Color[] { Color.yellow, Color.blue, Color.red, Color.black, Color.black };

	public OnDrawGizmosTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileGroup = m_GameContext.GetGroup(GameMatcher.TilePosition);
	}


	public void Update()
	{
		foreach (var tileEntity in m_TileGroup.GetEntities())
		{
			if (tileEntity.HasOwnable && tileEntity.Ownable.HasOwner)
			{
				int ownerId = tileEntity.Ownable.OwnerId;
				Gizmos.color = m_TileOwnerColors[ownerId % m_TileOwnerColors.Length];
			}
			else
			{
				Gizmos.color = Color.white;
			}

			Vector3 worldPos = GameConstants.TilePositionToWorldPosition(tileEntity.TilePosition.Value);
			Gizmos.DrawCube(worldPos, Vector3.one * 0.2f);
		}
	}
}
