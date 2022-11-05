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
}
