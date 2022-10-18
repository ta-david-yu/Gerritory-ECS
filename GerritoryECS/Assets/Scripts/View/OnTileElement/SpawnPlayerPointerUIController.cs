using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpawnPlayerPointerUIController : 
	EntityCreationEventListenerBase, 
	ITeamScoreAddedListener, 
	IEnterStateAddedListener, 
	ILeaveStateAddedListener, 
	IDeadAddedListener,
	IDeadRemovedListener,
	ITimerAddedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[SerializeField]
	private PlayerPointerUIAnimationController m_PlayerPointerUIPrefab;

	private Contexts m_Contexts;
	private PlayerPointerUIAnimationController m_SpawnedPlayerPointerUI;
	private float m_CurrentStateDuration;

	private const float k_UIDepthOrderBaseWhenAlive = -0.2f;
	private const float k_UIDepthOrderOffset = 0.05f;
	private const float k_UIDepthOrderWhenDead = 0.2f;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		m_Contexts = contexts;
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
		// Spawn pointer UI view
		m_SpawnedPlayerPointerUI = Instantiate(m_PlayerPointerUIPrefab);
		m_SpawnedPlayerPointerUI.SetFollowingTargetTransform(transform);
		m_SpawnedPlayerPointerUI.SnapToTargetTransform();

		ElementEntity elementEntity = entity as ElementEntity;
		if (elementEntity.HasTeam)
		{
			// Register team related events (i.e. score).
			LevelEntity teamEntity = contexts.Level.GetEntityWithTeamInfo(elementEntity.Team.Id);
			teamEntity.AddTeamScoreAddedListener(this);

			m_SpawnedPlayerPointerUI.SetNormalRingColor(m_ColorPalette.GetPlayerBodyColorForTeam(elementEntity.Team.Id));
		}

		// Register to player state related events.
		elementEntity.AddEnterStateAddedListener(this);
		elementEntity.AddLeaveStateAddedListener(this);

		// Register to dead events.
		elementEntity.AddDeadAddedListener(this);
		elementEntity.AddDeadRemovedListener(this);

		// Change UI depth value based on the element id to avoid overlapping issue.
		m_SpawnedPlayerPointerUI.SetZValue(k_UIDepthOrderBaseWhenAlive + elementEntity.OnTileElement.Id * k_UIDepthOrderOffset);

		// Update Timer progress to 0 initially.
		m_SpawnedPlayerPointerUI.ChangeTimerColor(Color.white);
		m_SpawnedPlayerPointerUI.UpdateTimerProgress(0.0f);

	}

	public void OnTeamScoreAdded(LevelEntity entity, int value)
	{
		m_SpawnedPlayerPointerUI.ChangeScoreText(value.ToString());
	}

	public void OnEnterStateAdded(ElementEntity entity)
	{
		var playerStateEntitiesSet = m_Contexts.PlayerState.GetEntitiesWithState(entity.StateHolder.Id);
		if (playerStateEntitiesSet.Count == 0)
		{
			Debug.LogWarning($"Couldn't find a player state that is associated to StateHolder {entity.StateHolder.Id}");
			return;
		}

		PlayerStateEntity playerStateEntity = playerStateEntitiesSet.SingleEntity();

		// Change the color based on the state.
		Color stateColor = Color.grey;
		if (playerStateEntity.HasStateFactoryType)
		{
			m_Contexts.Config.GameConfig.value.StateTypeFactory.TryGetStateColor(playerStateEntity.StateFactoryType.Value, out stateColor);
			m_SpawnedPlayerPointerUI.ChangeTimerColor(stateColor);
		}
		else if (playerStateEntity.HasWaitingForRespawnState)
		{
			// Waiting for respawn color.
			stateColor = Color.grey;
		}
		else
		{
			// Default fallback color.
			stateColor = Color.grey;
		}

		m_SpawnedPlayerPointerUI.PlayChangeStateAnimation(stateColor);
		m_SpawnedPlayerPointerUI.ChangeTimerColor(stateColor);
		m_SpawnedPlayerPointerUI.UpdateTimerProgress(1.0f);
		if (!playerStateEntity.HasTimer)
		{
			return;
		}

		// Register to the timer event so we could update the UI.
		m_CurrentStateDuration = playerStateEntity.Timer.Value;
		playerStateEntity.AddTimerAddedListener(this);
	}

	public void OnLeaveStateAdded(ElementEntity entity)
	{
		m_SpawnedPlayerPointerUI.PlayLeaveStateAnimation();

		m_SpawnedPlayerPointerUI.ChangeTimerColor(Color.white);
		m_SpawnedPlayerPointerUI.UpdateTimerProgress(0.0f);
	}

	public void OnTimerAdded(PlayerStateEntity entity, float value)
	{
		float progress = value / m_CurrentStateDuration;
		m_SpawnedPlayerPointerUI.UpdateTimerProgress(progress);
	}

	public void OnDeadAdded(ElementEntity entity)
	{
		m_SpawnedPlayerPointerUI.PlayDeadAnimation();
		m_SpawnedPlayerPointerUI.SetZValue(k_UIDepthOrderWhenDead);

	}

	public void OnDeadRemoved(ElementEntity entity)
	{
		m_SpawnedPlayerPointerUI.PlayRespawnAnimation();

		// Reset the z order back
		m_SpawnedPlayerPointerUI.SetZValue(k_UIDepthOrderBaseWhenAlive + entity.OnTileElement.Id * k_UIDepthOrderOffset);
	}
}
