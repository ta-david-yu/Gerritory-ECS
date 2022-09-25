using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintEntityCreationEventController : MonoBehaviour
{
	[SerializeField]
	private List<BlueprintEntityCreationEventListenerBase> m_Listeners = new List<BlueprintEntityCreationEventListenerBase>();

	public void OnEntityCreated(IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnEntityCreated(entity);
		}
	}

	public void OnBlueprintApplied(IEntity entity)
	{
		foreach (var listener in m_Listeners)
		{
			listener.HandleOnBlueprintApplied(entity);
		}
	}

	[ContextMenu("Collect All Listeners on GameObject or Children")]
	private void collectAllListenersInChildren()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.RecordObject(this, "Collect BlueprintEntityCreationEventListeners");
		m_Listeners.Clear();
		m_Listeners.AddRange(GetComponentsInChildren<BlueprintEntityCreationEventListenerBase>());
#endif
	}
}
