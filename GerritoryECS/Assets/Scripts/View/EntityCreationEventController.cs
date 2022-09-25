using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCreationEventController : MonoBehaviour, IEntityCreationEventController
{
	[SerializeField]
	private List<EntityCreationEventListenerBase> m_Listeners = new List<EntityCreationEventListenerBase>();

	public void OnEntityCreated(IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnEntityCreated(entity);
		}
	}

	public void OnComponentsAdded(IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnComponentsAdded(entity);
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
}
