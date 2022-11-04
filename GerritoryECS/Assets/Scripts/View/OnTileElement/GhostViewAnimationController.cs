using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostViewAnimationController : EntityCreationEventListenerBase, IGhostAppearingAddedListener, IGhostDisappearingAddedListener
{
	[SerializeField]
	private Renderer m_BodyRenderer;

	private Contexts m_CachedContexts;
	private ElementEntity m_CachedEntity;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		ElementEntity elementEntity = entity as ElementEntity;
		elementEntity.AddGhostAppearingAddedListener(this);
		elementEntity.AddGhostDisappearingAddedListener(this);

		m_BodyRenderer.material.color = Color.black;

		m_CachedContexts = contexts;
		m_CachedEntity = elementEntity;
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
	}

	public void OnGhostAppearingAdded(ElementEntity entity, float progress)
	{
		m_BodyRenderer.material.color = Color.Lerp(Color.black, Color.white, progress);
	}

	public void OnGhostDisappearingAdded(ElementEntity entity, float progress)
	{
		m_BodyRenderer.material.color = Color.Lerp(Color.white, Color.black, progress);
	}

	[ContextMenu("Make ghost disappear")]
	private void _makeDisappear()
	{
		m_CachedEntity.AddGhostDisappearing(0);

		// Kill the ghost
		TryCommandKillResult killResult = m_CachedContexts.TryCommandForceKillImmortal(m_CachedEntity);
		if (!killResult.Success)
		{
			// The kill action is not successful.
		}
	}


	[ContextMenu("Make ghost appear")]
	private void _makeReappear()
	{
		m_CachedEntity.AddGhostAppearing(0);

		// Get a position to revive the ghost on.
		TryGetValidRespawnPositionResult result = m_CachedContexts.TryGetValidGhostSpawnPosition(m_CachedEntity, 0);
		if (result.Success)
		{
			m_CachedEntity.AddOnTilePosition(result.TilePosition);
		}
		else
		{
			Debug.LogError("Cannot find a valid position to spawn the ghost, place the ghost at (0, 0)");
			m_CachedEntity.AddOnTilePosition(Vector2Int.zero);
		}
		m_CachedEntity.IsDead = false;
	}
}
