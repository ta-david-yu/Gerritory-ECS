//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class OnTilePositionAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<ElementEntity>
{
	readonly System.Collections.Generic.List<IOnTilePositionAddedListener> _listenerBuffer;

	public OnTilePositionAddedEventSystem(Contexts contexts) : base(contexts.Element)
	{
		_listenerBuffer = new System.Collections.Generic.List<IOnTilePositionAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<ElementEntity> GetTrigger(JCMG.EntitasRedux.IContext<ElementEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(ElementMatcher.OnTilePosition)
		);
	}

	protected override bool Filter(ElementEntity entity)
	{
		return entity.HasOnTilePosition && entity.HasOnTilePositionAddedListener;
	}

	protected override void Execute(System.Collections.Generic.List<ElementEntity> entities)
	{
		foreach (var e in entities)
		{
			var component = e.OnTilePosition;
			_listenerBuffer.Clear();
			_listenerBuffer.AddRange(e.OnTilePositionAddedListener.value);
			foreach (var listener in _listenerBuffer)
			{
				listener.OnOnTilePositionAdded(e, component.Value);
			}
		}
	}
}
