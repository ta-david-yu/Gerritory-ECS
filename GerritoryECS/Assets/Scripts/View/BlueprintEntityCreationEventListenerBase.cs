using JCMG.EntitasRedux;
using UnityEngine;

public abstract class BlueprintEntityCreationEventListenerBase : MonoBehaviour
{
	public abstract void HandleOnEntityCreated(IEntity entity);
	public abstract void HandleOnBlueprintApplied(IEntity entity);
}
