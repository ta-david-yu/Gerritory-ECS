//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class AnyTeamGameRankingAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<LevelEntity>
{
	readonly JCMG.EntitasRedux.IGroup<LevelEntity> _listeners;
	readonly System.Collections.Generic.List<LevelEntity> _entityBuffer;
	readonly System.Collections.Generic.List<IAnyTeamGameRankingAddedListener> _listenerBuffer;

	public AnyTeamGameRankingAddedEventSystem(Contexts contexts) : base(contexts.Level)
	{
		_listeners = contexts.Level.GetGroup(LevelMatcher.AnyTeamGameRankingAddedListener);
		_entityBuffer = new System.Collections.Generic.List<LevelEntity>();
		_listenerBuffer = new System.Collections.Generic.List<IAnyTeamGameRankingAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<LevelEntity> GetTrigger(JCMG.EntitasRedux.IContext<LevelEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(LevelMatcher.TeamGameRanking)
		);
	}

	protected override bool Filter(LevelEntity entity)
	{
		return entity.HasTeamGameRanking;
	}

	protected override void Execute(System.Collections.Generic.List<LevelEntity> entities)
	{
		foreach (var e in entities)
		{
			var component = e.TeamGameRanking;
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.AnyTeamGameRankingAddedListener.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.OnAnyTeamGameRankingAdded(e, component.Number);
				}
			}
		}
	}
}