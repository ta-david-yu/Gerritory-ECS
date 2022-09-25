using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAnimationController : EntityCreationEventListenerBase, IDeadAddedListener, IDeadRemovedListener
{
	[SerializeField]
	private GameObject m_Appearance;

	/// <summary>
	/// For debug purpose
	/// </summary>
	private GameEntity m_CahcedGameEntity;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		// Register listener to relevant components
		gameEntity.AddDeadAddedListener(this);
		gameEntity.AddDeadRemovedListener(this);

		m_CahcedGameEntity = gameEntity;
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;
		m_Appearance.SetActive(!gameEntity.IsDead);
	}

	public void OnDeadAdded(GameEntity entity)
	{
		m_Appearance.SetActive(false);
	}

	public void OnDeadRemoved(GameEntity entity)
	{
		m_Appearance.SetActive(true);
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

		TryKillResult killResult = Contexts.SharedInstance.TryKill(m_CahcedGameEntity);
	}
}
