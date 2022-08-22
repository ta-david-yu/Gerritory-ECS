using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class DebugMessageSystem : ReactiveSystem<GameEntity>
{
	public DebugMessageSystem(Contexts contexts) : base(contexts.Game)
	{

	}

	protected override void Execute(List<GameEntity> entities)
	{
		foreach (var entity in entities)
		{
			Debug.Log(entity.DebugMessage.Message);
		}
	}

	protected override bool Filter(GameEntity entity)
	{
		// Do a final check in case the entity has been changed in a different system
		return entity.HasDebugMessage;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.DebugMessage);
	}
}
