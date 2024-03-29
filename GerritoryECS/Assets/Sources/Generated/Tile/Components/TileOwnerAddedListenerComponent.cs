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
	public OwnerAddedListenerComponent OwnerAddedListener { get { return (OwnerAddedListenerComponent)GetComponent(TileComponentsLookup.OwnerAddedListener); } }
	public bool HasOwnerAddedListener { get { return HasComponent(TileComponentsLookup.OwnerAddedListener); } }

	public void AddOwnerAddedListener(System.Collections.Generic.List<IOwnerAddedListener> newValue)
	{
		var index = TileComponentsLookup.OwnerAddedListener;
		var component = (OwnerAddedListenerComponent)CreateComponent(index, typeof(OwnerAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceOwnerAddedListener(System.Collections.Generic.List<IOwnerAddedListener> newValue)
	{
		var index = TileComponentsLookup.OwnerAddedListener;
		var component = (OwnerAddedListenerComponent)CreateComponent(index, typeof(OwnerAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyOwnerAddedListenerTo(OwnerAddedListenerComponent copyComponent)
	{
		var index = TileComponentsLookup.OwnerAddedListener;
		var component = (OwnerAddedListenerComponent)CreateComponent(index, typeof(OwnerAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveOwnerAddedListener()
	{
		RemoveComponent(TileComponentsLookup.OwnerAddedListener);
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
	static JCMG.EntitasRedux.IMatcher<TileEntity> _matcherOwnerAddedListener;

	public static JCMG.EntitasRedux.IMatcher<TileEntity> OwnerAddedListener
	{
		get
		{
			if (_matcherOwnerAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TileEntity>)JCMG.EntitasRedux.Matcher<TileEntity>.AllOf(TileComponentsLookup.OwnerAddedListener);
				matcher.ComponentNames = TileComponentsLookup.ComponentNames;
				_matcherOwnerAddedListener = matcher;
			}

			return _matcherOwnerAddedListener;
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
	public void AddOwnerAddedListener(IOwnerAddedListener value)
	{
		var listeners = HasOwnerAddedListener
			? OwnerAddedListener.value
			: new System.Collections.Generic.List<IOwnerAddedListener>();
		listeners.Add(value);
		ReplaceOwnerAddedListener(listeners);
	}

	public void RemoveOwnerAddedListener(IOwnerAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = OwnerAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveOwnerAddedListener();
		}
		else
		{
			ReplaceOwnerAddedListener(listeners);
		}
	}
}
