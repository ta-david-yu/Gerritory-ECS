using JCMG.EntitasRedux;
using UnityEngine;
using UnityEngine.Events;

public class EntityCreationUnityEvent : EntityCreationEventListenerBase
{
	[System.Serializable]
	public class ExecuteEvents
	{
		[Tooltip("Invoked when the entity is created")]
		public UnityEvent<IEntity> OnEntityCreated;

		[Tooltip("Invoked when initial components are applied to the created entity")]
		public UnityEvent<IEntity> OnComponentsAdded;
	}

	public ExecuteEvents Events = new ExecuteEvents();

	public override void HandleOnEntityCreated(IEntity entity)
	{
		Events.OnEntityCreated?.Invoke(entity);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		Events.OnComponentsAdded?.Invoke(entity);
	}
}
