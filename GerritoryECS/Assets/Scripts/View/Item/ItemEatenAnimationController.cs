using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEatenAnimationController : EntityCreationEventListenerBase, IEatenAddedListener
{
	[SerializeField]
	private GameObject m_ViewRootGameObject;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		ItemEntity itemEntity = entity as ItemEntity;

		// Register listener to relevant components
		itemEntity.AddEatenAddedListener(this);
		itemEntity.OnDestroyEntity += handleOnEntityDestroyed;
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
	}

	public void OnEatenAdded(ItemEntity entity, int eaterId)
	{
		m_ViewRootGameObject.Unlink();
		Destroy(m_ViewRootGameObject);
	}

	private void handleOnEntityDestroyed(IEntity entity)
	{
		m_ViewRootGameObject.Unlink();
		Destroy(m_ViewRootGameObject);
	}
}
