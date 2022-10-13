using JCMG.EntitasRedux;
using UnityEngine;

public abstract class EntityCreationEventListenerBase : MonoBehaviour
{
	public abstract void HandleOnEntityCreated(Contexts contexts, IEntity entity);
	public abstract void HandleOnComponentsAdded(Contexts contexts, IEntity entity);
}
