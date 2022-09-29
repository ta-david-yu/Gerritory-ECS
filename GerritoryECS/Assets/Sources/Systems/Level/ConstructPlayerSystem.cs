using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build player entity on request.
/// </summary>
public sealed class ConstructPlayerSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<LevelEntity> m_ConstructPlayerRequestGroup;

	public ConstructPlayerSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;

		m_ConstructPlayerRequestGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructPlayer);
	}

	public void FixedUpdate()
	{
		foreach (var constructPlayerRequest in m_ConstructPlayerRequestGroup.GetEntities())
		{
			ConstructPlayerComponent constructPlayerComponent = constructPlayerRequest.ConstructPlayer;
			ElementEntity playerEntity = createPlayerEntity(constructPlayerComponent);

			constructPlayerRequest.Destroy();
		}
	}

	private ElementEntity createPlayerEntity(ConstructPlayerComponent constructPlayerRequest)
	{
		IPlayerFactory playerFactory = m_ConfigContext.GameConfig.value.PlayerFactory;

		// Create player entity and its view controller
		ElementEntity playerEntity = m_ElementContext.CreateEntity();
		IEntityCreationEventController viewController = playerFactory.CreatePlayerView(constructPlayerRequest.PlayerId, constructPlayerRequest.TeamId, constructPlayerRequest.SkinId);
		viewController.OnEntityCreated(playerEntity);
		
		// Add needed componenets
		playerEntity.AddPlayer(constructPlayerRequest.PlayerId);
		playerEntity.AddTeam(constructPlayerRequest.TeamId);
		playerEntity.AddOnTileElement(m_LevelContext.GetNewOnTileElementId());
		playerEntity.AddItemEater(m_LevelContext.GetNewItemEaterId());
		playerEntity.AddStateHolder(m_LevelContext.GetNewStateHolderId());
		playerEntity.AddSpeedChangeable(1, 1);
		playerEntity.IsTileOwner = true;
		playerEntity.IsTileCollapser = true;
		playerEntity.IsCanBeDead = true;
		playerEntity.IsCanRespawnAfterDeath = true;
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

		m_MessageContext.EmitOnTileElementEnterTileMessage(playerEntity.OnTileElement.Id, playerEntity.OnTilePosition.Value);

		viewController.OnComponentsAdded(playerEntity);

		// Link view controller with entity
		viewController.Link(playerEntity);

		return playerEntity;
	}
}
