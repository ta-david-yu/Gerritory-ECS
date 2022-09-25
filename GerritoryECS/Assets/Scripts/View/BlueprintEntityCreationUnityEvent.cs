using JCMG.EntitasRedux;
using UnityEngine;
using UnityEngine.Events;

public class BlueprintEntityCreationUnityEvent : BlueprintEntityCreationEventListenerBase
{
	[System.Serializable]
	public class ExecuteEvents
	{
		[Tooltip("Invoked when the entity is created")]
		public UnityEvent<IEntity> OnEntityCreated;

		[Tooltip("Invoked when the given blueprint is applied to the created entity")]
		public UnityEvent<IEntity> OnBlueprintApplied;
	}

	public ExecuteEvents Events = new ExecuteEvents();

	public override void HandleOnEntityCreated(IEntity entity)
	{
		Events.OnEntityCreated?.Invoke(entity);
	}

	public override void HandleOnBlueprintApplied(IEntity entity)
	{
		Events.OnBlueprintApplied?.Invoke(entity);
	}
}
