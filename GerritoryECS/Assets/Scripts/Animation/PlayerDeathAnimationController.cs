using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAnimationController : MonoBehaviour, IDeadAddedListener, IDeadRemovedListener
{
	[SerializeField]
	private GameObject m_Appearance;

	/// <summary>
	/// For debug purpose
	/// </summary>
	private GameEntity m_CahcedGameEntity;

	public void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		// Register listener to relevant components
		gameEntity.AddDeadAddedListener(this);
		gameEntity.AddDeadRemovedListener(this);

		m_CahcedGameEntity = gameEntity;
	}

	public void HandleOnBlueprintApplied(IEntity entity)
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

		m_CahcedGameEntity.IsDead = true;
		m_CahcedGameEntity.RemoveOnTilePosition();
	}
}
