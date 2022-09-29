using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class DebugMessageSystem : ReactiveSystem<ElementEntity>
{
	public DebugMessageSystem(Contexts contexts) : base(contexts.Element)
	{

	}

	protected override void Execute(List<ElementEntity> entities)
	{
		foreach (var entity in entities)
		{
			Debug.Log(entity.DebugMessage.Message);
		}
	}

	protected override bool Filter(ElementEntity entity)
	{
		// Do a final check in case the entity has been changed in a different system
		return entity.HasDebugMessage;
	}

	protected override ICollector<ElementEntity> GetTrigger(IContext<ElementEntity> context)
	{
		return context.CreateCollector(ElementMatcher.DebugMessage);
	}
}
