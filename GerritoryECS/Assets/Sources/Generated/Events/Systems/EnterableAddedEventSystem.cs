//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class EnterableAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<TileEntity>
{
	readonly System.Collections.Generic.List<IEnterableAddedListener> _listenerBuffer;

	public EnterableAddedEventSystem(Contexts contexts) : base(contexts.Tile)
	{
		_listenerBuffer = new System.Collections.Generic.List<IEnterableAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<TileEntity> GetTrigger(JCMG.EntitasRedux.IContext<TileEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(TileMatcher.Enterable)
		);
	}

	protected override bool Filter(TileEntity entity)
	{
		return entity.IsEnterable && entity.HasEnterableAddedListener;
	}

	protected override void Execute(System.Collections.Generic.List<TileEntity> entities)
	{
		foreach (var e in entities)
		{
			
			_listenerBuffer.Clear();
			_listenerBuffer.AddRange(e.EnterableAddedListener.value);
			foreach (var listener in _listenerBuffer)
			{
				listener.OnEnterableAdded(e);
			}
		}
	}
}
