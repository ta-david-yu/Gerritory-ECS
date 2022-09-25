using JCMG.EntitasRedux;
using UnityEngine;

public abstract class EntityCreationEventListenerBase : MonoBehaviour
{
	public abstract void HandleOnEntityCreated(IEntity entity);
	public abstract void HandleOnComponentsAdded(IEntity entity);
}
