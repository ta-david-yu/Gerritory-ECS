using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InGameStateMachineSystem : IInitializeSystem, IFixedUpdateSystem
{
	private readonly GameFlowContext m_GameFlowContext;
	private readonly LevelContext m_LevelContext;
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly CommandContext m_CommandContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ElementEntity> m_OnTileElementEntityGroup;

	public InGameStateMachineSystem(Contexts contexts)
	{
		m_GameFlowContext = contexts.GameFlow;
		m_LevelContext = contexts.Level;
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_CommandContext = contexts.Command;
		m_ConfigContext = contexts.Config;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;

		m_OnTileElementEntityGroup = m_ElementContext.GetGroup(ElementMatcher.OnTileElement);
	}

	public void Initialize()
	{
		m_GameFlowContext.IsGameFlow = true;
		m_GameFlowContext.GameFlowEntity.ReplaceInGameState(InGameStateComponent.State.Intro);
	}

	public void FixedUpdate()
	{
		float countdownSpeedMultiplier = 1.0f;
		GameFlowEntity gameFlowEntity = m_GameFlowContext.GameFlowEntity;
		InGameStateComponent.State state = gameFlowEntity.InGameState.Value;
		if (state == InGameStateComponent.State.Intro)
		{
			LevelData levelData = m_ConfigContext.GameConfig.value.LevelData;
			foreach (var tileDataPair in levelData.TileDataPairs)
			{
				// Create tile construction request entity.
				m_CommandContext.CreateEntity().AddConstructTile
				(
					tileDataPair.Key,
					tileDataPair.Value
				);
				//m_Contexts.ConstructTileEntityAtPosition(tileDataPair.Value, tileDataPair.Key);
			}

			var playerConfigs = m_ConfigContext.GameConfig.value.PlayerGameConfigs;
			foreach (var playerConfig in playerConfigs)
			{
				// Create player construction request entity.
				m_CommandContext.CreateEntity().AddConstructPlayer
				(
					playerConfig.PlayerId,
					playerConfig.PlayerName,
					playerConfig.TeamId,
					playerConfig.SkinId
				);
				//m_Contexts.ConstructPlayerEntity(playerConfig.PlayerId, playerConfig.TeamId, playerConfig.SkinId);
			}

			gameFlowEntity.AddCountdownTimer(3);

			// Go to countdown directly.
			gameFlowEntity.ReplaceInGameState(InGameStateComponent.State.Countdown);
		}
		else if (state == InGameStateComponent.State.Countdown)
		{
			float countdownTimer = gameFlowEntity.CountdownTimer.Value;
			countdownTimer -= countdownSpeedMultiplier * Time.fixedDeltaTime;

			if (countdownTimer > 0)
			{
				gameFlowEntity.ReplaceCountdownTimer(countdownTimer);
			}
			else
			{
				// Spawn input entity so the user/AI can start controlling their player entity.
				var playerConfigs = m_ConfigContext.GameConfig.value.PlayerGameConfigs;
				foreach (var playerConfig in playerConfigs)
				{
					if (!playerConfig.IsAI)
					{
						m_CommandContext.CreateEntity().AddConstructUserInput(playerConfig.PlayerId, playerConfig.PlayerId);
					}
					else
					{
						m_CommandContext.CreateEntity().AddConstructAIInput(Movement.Type.Right, playerConfig.PlayerId);
					}
				}

				m_LevelContext.GameInfoEntity.AddGameTimer(0);
				gameFlowEntity.RemoveCountdownTimer();

				// Go to play after countdown timer is up.
				gameFlowEntity.ReplaceInGameState(InGameStateComponent.State.Playing);
				gameFlowEntity.IsPlaying = true;
			}
		}
		else if (state == InGameStateComponent.State.Playing)
		{
			if (!gameFlowEntity.IsGameOver)
			{
				return;
			}

			// Destory all the input entities after the game is over.
			m_InputContext.DestroyAllEntities();

			// Remove components that might change the result of the game.
			foreach (var onTileEntity in m_OnTileElementEntityGroup)
			{
				if (onTileEntity.IsCanBeDead)
				{
					onTileEntity.IsCanBeDead = false;
				}

				if (onTileEntity.IsTileOwner)
				{
					onTileEntity.IsTileOwner = false;
				}
			}

			// Go to outro after the game is done.
			gameFlowEntity.ReplaceInGameState(InGameStateComponent.State.Outro);
			gameFlowEntity.IsPlaying = false;
		}
	}
}
