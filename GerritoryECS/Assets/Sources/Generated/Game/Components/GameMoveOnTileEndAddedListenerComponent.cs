//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity
{
	public MoveOnTileEndAddedListenerComponent MoveOnTileEndAddedListener { get { return (MoveOnTileEndAddedListenerComponent)GetComponent(GameComponentsLookup.MoveOnTileEndAddedListener); } }
	public bool HasMoveOnTileEndAddedListener { get { return HasComponent(GameComponentsLookup.MoveOnTileEndAddedListener); } }

	public void AddMoveOnTileEndAddedListener(System.Collections.Generic.List<IMoveOnTileEndAddedListener> newValue)
	{
		var index = GameComponentsLookup.MoveOnTileEndAddedListener;
		var component = (MoveOnTileEndAddedListenerComponent)CreateComponent(index, typeof(MoveOnTileEndAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMoveOnTileEndAddedListener(System.Collections.Generic.List<IMoveOnTileEndAddedListener> newValue)
	{
		var index = GameComponentsLookup.MoveOnTileEndAddedListener;
		var component = (MoveOnTileEndAddedListenerComponent)CreateComponent(index, typeof(MoveOnTileEndAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMoveOnTileEndAddedListenerTo(MoveOnTileEndAddedListenerComponent copyComponent)
	{
		var index = GameComponentsLookup.MoveOnTileEndAddedListener;
		var component = (MoveOnTileEndAddedListenerComponent)CreateComponent(index, typeof(MoveOnTileEndAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMoveOnTileEndAddedListener()
	{
		RemoveComponent(GameComponentsLookup.MoveOnTileEndAddedListener);
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
public sealed partial class GameMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherMoveOnTileEndAddedListener;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> MoveOnTileEndAddedListener
	{
		get
		{
			if (_matcherMoveOnTileEndAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.MoveOnTileEndAddedListener);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherMoveOnTileEndAddedListener = matcher;
			}

			return _matcherMoveOnTileEndAddedListener;
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
public partial class GameEntity
{
	public void AddMoveOnTileEndAddedListener(IMoveOnTileEndAddedListener value)
	{
		var listeners = HasMoveOnTileEndAddedListener
			? MoveOnTileEndAddedListener.value
			: new System.Collections.Generic.List<IMoveOnTileEndAddedListener>();
		listeners.Add(value);
		ReplaceMoveOnTileEndAddedListener(listeners);
	}

	public void RemoveMoveOnTileEndAddedListener(IMoveOnTileEndAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = MoveOnTileEndAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveMoveOnTileEndAddedListener();
		}
		else
		{
			ReplaceMoveOnTileEndAddedListener(listeners);
		}
	}
}