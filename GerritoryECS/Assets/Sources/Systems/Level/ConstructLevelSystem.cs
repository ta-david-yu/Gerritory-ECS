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
	private readonly TileContext m_TileContext;
	private readonly ConfigContext m_ConfigContext;

	public ConstructLevelSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
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
				var tileEntity = m_TileContext.CreateEntity();
				tileEntity.AddTilePosition(new Vector2Int(x, y));
				tileEntity.IsEnterable = true;
				tileEntity.IsItemHolder = true;

				if (Random.Range(0.0f, 1.0f) > 0.0f)
				{
					tileEntity.AddCollapseOnStepped(1);
				}
				else
				{
					tileEntity.AddOwnable(false, -1);
				}
			}
		}
	}
}
