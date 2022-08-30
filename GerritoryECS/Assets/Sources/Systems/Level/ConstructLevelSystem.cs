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
				if (Random.Range(0.0f, 1.0f) < 0.2f)
				{
					// TODO: Removed this testing enterable line
					continue;
				}

				var tileEntity = m_GameContext.CreateEntity();
				tileEntity.AddTilePosition(new Vector2Int(x, y));

				// TODO: Removed this testing enterable line
				tileEntity.IsEnterable = Random.Range(0.0f, 1.0f) > 0.2f;
			}
		}
	}
}
