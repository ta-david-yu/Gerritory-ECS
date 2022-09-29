using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InGameStateMachineSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly GameContext m_GameContext;
	private readonly InputContext m_InputContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	public InGameStateMachineSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_GameContext = contexts.Game;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;
	}

	public void Initialize()
	{
		var playerConfigs = m_ConfigContext.GameConfig.value.PlayerGameConfigs;
		foreach (var playerConfig in playerConfigs)
		{
			// Create player construction request entity.
			m_LevelContext.CreateEntity().AddConstructPlayer
			(
				playerConfig.PlayerId, 
				playerConfig.PlayerName, 
				playerConfig.TeamId, 
				playerConfig.SkinId, 
				playerConfig.IsAI
			);
		}

		LevelData levelData = m_ConfigContext.GameConfig.value.LevelData;
		foreach (var tileDataPair in levelData.TileDataPairs)
		{
			// Create tile construction request entity.
			m_LevelContext.CreateEntity().AddConstructTile
			(
				tileDataPair.Key,
				tileDataPair.Value
			);
		}
	}
}
