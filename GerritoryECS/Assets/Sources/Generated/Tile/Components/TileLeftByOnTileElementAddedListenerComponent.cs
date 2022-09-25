//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TileEntity
{
	public LeftByOnTileElementAddedListenerComponent LeftByOnTileElementAddedListener { get { return (LeftByOnTileElementAddedListenerComponent)GetComponent(TileComponentsLookup.LeftByOnTileElementAddedListener); } }
	public bool HasLeftByOnTileElementAddedListener { get { return HasComponent(TileComponentsLookup.LeftByOnTileElementAddedListener); } }

	public void AddLeftByOnTileElementAddedListener(System.Collections.Generic.List<ILeftByOnTileElementAddedListener> newValue)
	{
		var index = TileComponentsLookup.LeftByOnTileElementAddedListener;
		var component = (LeftByOnTileElementAddedListenerComponent)CreateComponent(index, typeof(LeftByOnTileElementAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceLeftByOnTileElementAddedListener(System.Collections.Generic.List<ILeftByOnTileElementAddedListener> newValue)
	{
		var index = TileComponentsLookup.LeftByOnTileElementAddedListener;
		var component = (LeftByOnTileElementAddedListenerComponent)CreateComponent(index, typeof(LeftByOnTileElementAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyLeftByOnTileElementAddedListenerTo(LeftByOnTileElementAddedListenerComponent copyComponent)
	{
		var index = TileComponentsLookup.LeftByOnTileElementAddedListener;
		var component = (LeftByOnTileElementAddedListenerComponent)CreateComponent(index, typeof(LeftByOnTileElementAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveLeftByOnTileElementAddedListener()
	{
		RemoveComponent(TileComponentsLookup.LeftByOnTileElementAddedListener);
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class TileMatcher
{
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherLeftByOnTileElementAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> LeftByOnTileElementAddedListener
	{
		get
		{
			if (_matcherLeftByOnTileElementAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.LeftByOnTileElementAddedListener);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherLeftByOnTileElementAddedListener = matcher;
			}

			return _matcherLeftByOnTileElementAddedListener;
		}
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TileEntity
{
	public void AddLeftByOnTileElementAddedListener(ILeftByOnTileElementAddedListener value)
	{
		var listeners = HasLeftByOnTileElementAddedListener
			? LeftByOnTileElementAddedListener.value
			: new System.Collections.Generic.List<ILeftByOnTileElementAddedListener>();
		listeners.Add(value);
		ReplaceLeftByOnTileElementAddedListener(listeners);
	}

	public void RemoveLeftByOnTileElementAddedListener(ILeftByOnTileElementAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = LeftByOnTileElementAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveLeftByOnTileElementAddedListener();
		}
		else
		{
			ReplaceLeftByOnTileElementAddedListener(listeners);
		}
	}
}