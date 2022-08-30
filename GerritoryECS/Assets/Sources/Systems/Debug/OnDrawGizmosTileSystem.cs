using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosTileSystem : IUpdateSystem
{
	private readonly GameContext m_GameContext;
	private IGroup<GameEntity> m_TileGroup;

	public OnDrawGizmosTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileGroup = m_GameContext.GetGroup(GameMatcher.TilePosition);
	}


	public void Update()
	{
		foreach (var tileEntity in m_TileGroup.GetEntities())
		{
			if (tileEntity.IsEnterable)
			{
				Gizmos.color = Color.blue;
			}
			else
			{
				Gizmos.color = Color.red;
			}

			Vector3 worldPos = GameConstants.TilePositionToWorldPosition(tileEntity.TilePosition.Value);
			Gizmos.DrawCube(worldPos, Vector3.one * 0.2f);
		}
	}
}
