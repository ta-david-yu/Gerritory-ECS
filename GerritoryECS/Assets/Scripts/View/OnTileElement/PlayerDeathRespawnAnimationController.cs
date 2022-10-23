using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathRespawnAnimationController : EntityCreationEventListenerBase, IDeadAddedListener, IDeadRemovedListener
{
	[SerializeField]
	private GameObject m_Appearance;

	[SerializeField]
	private Transform m_RespawnAnimationRoot;

	[Header("Settings")]

	[SerializeField]
	private float m_RespawnAnimationStartScale = 0.0f;

	[SerializeField]
	private float m_RespawnAnimationEndScale = 1.0f;

	[SerializeField]
	private float m_RespawnAnimationDuration = 0.2f;

	[SerializeField]
	private EasingFunction.Ease m_RespawnAnimationEasingType = EasingFunction.Ease.EaseOutBackDouble;

	private Tweener m_RespawnTweener = null;

	/// <summary>
	/// For debug purpose
	/// </summary>
	private ElementEntity m_CahcedGameEntity;


	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		ElementEntity gameEntity = entity as ElementEntity;

		// Register listener to relevant components
		gameEntity.AddDeadAddedListener(this);
		gameEntity.AddDeadRemovedListener(this);

		m_CahcedGameEntity = gameEntity;
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
		ElementEntity gameEntity = entity as ElementEntity;
		m_Appearance.SetActive(!gameEntity.IsDead);
	}

	public void OnDeadAdded(ElementEntity entity)
	{
		// Dead animation
		m_Appearance.SetActive(false);
	}

	public void OnDeadRemoved(ElementEntity entity)
	{
		// Respawn animation
		m_Appearance.SetActive(true);

		Tweener.SafeAbortTweener(ref m_RespawnTweener);

		m_RespawnTweener = TweenManager.Instance.Tween((float progress) =>
		{
			Vector3 newScale = Vector3.one * Mathf.LerpUnclamped(m_RespawnAnimationStartScale, m_RespawnAnimationEndScale, progress);
			m_RespawnAnimationRoot.localScale = newScale;

		}).SetEase(m_RespawnAnimationEasingType).SetTime(m_RespawnAnimationDuration)
		.SetEndCallback(() =>
		{
			m_RespawnTweener = null;
		});
	}

	[ContextMenu("Kill")]
	private void killPlayer()
	{
		if (m_CahcedGameEntity == null)
		{
			Debug.LogWarning("The entity reference is null. Cannot kill the player entity.");
			return;
		}

		if (Contexts.SharedInstance == null)
		{
			Debug.LogWarning("Contexts non-existent, cannot kill the player.");
			return;
		}

		TryCommandKillResult killResult = Contexts.SharedInstance.TryCommandKill(m_CahcedGameEntity);
	}
}
