using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build the level based on the GameConfig instance
/// </summary>
public sealed class ConstructLevelSystem : IInitializeSystem
{
	private readonly GameContext m_GameContext;
	private readonly ConfigContext m_ConfigContext;

	public ConstructLevelSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		Vector2Int levelSize = m_ConfigContext.GameConfig.value.LevelSize;
		m_GameContext.SetLevel(levelSize);

		for (int x = 0; x < levelSize.x; x++)
		{
			for (int y = 0; y < levelSize.y; y++)
			{
				var tileEntity = m_GameContext.CreateEntity();
				tileEntity.AddTilePosition(new Vector2Int(x, y));
				tileEntity.IsEnterable = true;
				tileEntity.AddOwnable(false, -1);
				tileEntity.AddCollapseOnStepped(1);
			}
		}
	}
}
