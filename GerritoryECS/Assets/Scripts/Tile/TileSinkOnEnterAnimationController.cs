using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSinkOnEnterAnimationController : EntityCreationEventListenerBase, IEnteredByOnTileElementAddedListener, ILeftByOnTileElementAddedListener
{
	private enum AnimState
	{
		Idle,
		Sinking,
		Rising
	}

	[SerializeField]
	private Transform m_Appearance;

	private const float k_HeightOffset = -0.3f;
	private const float k_SinkTime = 0.1f;
	private const float k_RiseTime = 0.3f;

	private AnimState m_State = AnimState.Idle;
	private float m_AnimTimer;

	private void Update()
	{
		float timeStep = Time.deltaTime;
		if (m_State == AnimState.Sinking)
		{
			m_AnimTimer -= timeStep;

			if (m_AnimTimer >= 0)
			{
				float progress = 1 - m_AnimTimer / k_SinkTime;
				var pos = m_Appearance.localPosition;
				pos.y = EasingFunction.EaseOutExpo(0.0f, 0.0f + k_HeightOffset, progress);
				m_Appearance.localPosition = pos;
			}
			else
			{
				var pos = m_Appearance.localPosition;
				pos.y = 0.0f + k_HeightOffset;
				m_Appearance.localPosition = pos;
				m_State = AnimState.Idle;
			}
		}
		else if (m_State == AnimState.Rising)
		{
			m_AnimTimer -= timeStep;

			if (m_AnimTimer >= 0)
			{
				float progress = 1 - m_AnimTimer / k_RiseTime;
				var pos = m_Appearance.localPosition;
				pos.y = EasingFunction.EaseOutSine(0.0f + k_HeightOffset, 0.0f, progress);
				m_Appearance.localPosition = pos;
			}
			else
			{
				var pos = m_Appearance.localPosition;
				pos.y = 0.0f;
				m_Appearance.localPosition = pos;
				m_State = AnimState.Idle;
			}
		}
	}

	public override void HandleOnEntityCreated(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddEnteredByOnTileElementAddedListener(this);
		tileEntity.AddLeftByOnTileElementAddedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
	}

	public void OnEnteredByOnTileElementAdded(TileEntity entity, int onTileElementId)
	{
		m_AnimTimer = k_SinkTime;
		m_State = AnimState.Sinking;
	}

	public void OnLeftByOnTileElementAdded(TileEntity entity, int onTileElementId)
	{
		m_AnimTimer = k_RiseTime;
		m_State = AnimState.Rising;
	}
}
