using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityEventListenerBehaviour : MonoBehaviour, IEntityEventListener
{
	public abstract void RegisterListenerToEntity(IEntity entity);
}
