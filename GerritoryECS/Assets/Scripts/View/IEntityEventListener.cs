using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public interface IEntityEventListener
{
	void RegisterListenerToEntity(IEntity entity);
}
