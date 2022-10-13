using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EntityCreationEventController : MonoBehaviour, IEntityCreationEventController
{
	[SerializeField]
	private List<EntityCreationEventListenerBase> m_Listeners = new List<EntityCreationEventListenerBase>();

	public void OnEntityCreated(Contexts contexts, IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnEntityCreated(contexts, entity);
		}
	}

	public void OnComponentsAdded(Contexts contexts, IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnComponentsAdded(contexts, entity);
		}
	}

	public void Link(IEntity entity)
	{
		gameObject.Link(entity);
	}

	[ContextMenu("Collect All Listeners on GameObject or Children")]
	private void collectAllListenersInChildren()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Collect EntityCreationEventListeners");
		m_Listeners.Clear();
		m_Listeners.AddRange(GetComponentsInChildren<EntityCreationEventListenerBase>());
#endif
	}

	[ContextMenu("Destroy & Unlink Entity")]
	private void destroyAndUnlinkEntity()
	{
		EntityLink entityLink = gameObject.GetEntityLink();
		if (entityLink == null)
		{
			Debug.LogWarning($"The gameObject {this.gameObject.name} was not linked to any entity!");
			return;
		}

		IEntity entity = entityLink.Entity;
		gameObject.Unlink();
		entity.Destroy();
	}
}
