using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConstructPlayerSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly GameContext m_GameContext;
	private readonly InputContext m_InputContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly Contexts m_Contexts;

	public ConstructPlayerSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_GameContext = contexts.Game;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;
		m_Contexts = contexts;
	}

	public void Initialize()
	{
		var playerConfigs = m_ConfigContext.GameConfig.value.PlayerGameConfigs;
		foreach (var playerConfig in playerConfigs)
		{
			GameEntity playerEntity = createPlayerEntity(playerConfig);

			InputEntity inputEntity = m_InputContext.CreateEntity();
			if (playerConfig.IsAI)
			{
				inputEntity.AddAIInput(Movement.Type.Right, playerConfig.PlayerId);
			}
			else
			{
				inputEntity.AddUserInput(playerConfig.PlayerId, playerConfig.PlayerId);
			}
		}
	}

	private GameEntity createPlayerEntity(PlayerGameConfig playerConfig)
	{
		IPlayerFactory playerFactory = m_ConfigContext.GameConfig.value.PlayerFactory;

		// Create player entity and its view controller
		GameEntity playerEntity = m_GameContext.CreateEntity();
		IEntityCreationEventController viewController = playerFactory.CreatePlayerView(playerConfig.PlayerId, playerConfig.ColorId, playerConfig.SkinId);
		viewController.OnEntityCreated(playerEntity);

		// Add needed componenets
		playerEntity.AddPlayer(playerConfig.PlayerId);
		playerEntity.AddOnTileElement(m_LevelContext.GetNewOnTileElementId());
		playerEntity.AddTileOwner(m_LevelContext.GetNewTileOwnerId(), 0);
		playerEntity.AddItemEater(m_LevelContext.GetNewItemEaterId());
		playerEntity.AddStateHolder(m_LevelContext.GetNewStateHolderId());
		playerEntity.AddSpeedChangeable(1, 1);
		playerEntity.IsTileCollapser = true;
		playerEntity.IsCanBeDead = true;
		playerEntity.IsOnTileElementKiller = true;

		TryGetValidRespawnPositionResult result = m_Contexts.TryGetValidRespawnPositionOfAreaIdFor(playerEntity, 0);
		if (result.Success)
		{
			playerEntity.AddOnTilePosition(result.TilePosition);
		}
		else
		{
			Debug.LogWarning("Cannot find a valid position to spawn the player, place the player at (0, 0)");
			playerEntity.AddOnTilePosition(Vector2Int.zero);
		}

		viewController.OnComponentsAdded(playerEntity);

		// Link view controller with entity
		viewController.Link(playerEntity);

		return playerEntity;
	}
}
