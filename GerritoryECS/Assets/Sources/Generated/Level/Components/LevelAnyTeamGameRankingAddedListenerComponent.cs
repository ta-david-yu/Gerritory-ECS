//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class LevelEntity
{
	public AnyTeamGameRankingAddedListenerComponent AnyTeamGameRankingAddedListener { get { return (AnyTeamGameRankingAddedListenerComponent)GetComponent(LevelComponentsLookup.AnyTeamGameRankingAddedListener); } }
	public bool HasAnyTeamGameRankingAddedListener { get { return HasComponent(LevelComponentsLookup.AnyTeamGameRankingAddedListener); } }

	public void AddAnyTeamGameRankingAddedListener(System.Collections.Generic.List<IAnyTeamGameRankingAddedListener> newValue)
	{
		var index = LevelComponentsLookup.AnyTeamGameRankingAddedListener;
		var component = (AnyTeamGameRankingAddedListenerComponent)CreateComponent(index, typeof(AnyTeamGameRankingAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceAnyTeamGameRankingAddedListener(System.Collections.Generic.List<IAnyTeamGameRankingAddedListener> newValue)
	{
		var index = LevelComponentsLookup.AnyTeamGameRankingAddedListener;
		var component = (AnyTeamGameRankingAddedListenerComponent)CreateComponent(index, typeof(AnyTeamGameRankingAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyAnyTeamGameRankingAddedListenerTo(AnyTeamGameRankingAddedListenerComponent copyComponent)
	{
		var index = LevelComponentsLookup.AnyTeamGameRankingAddedListener;
		var component = (AnyTeamGameRankingAddedListenerComponent)CreateComponent(index, typeof(AnyTeamGameRankingAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveAnyTeamGameRankingAddedListener()
	{
		RemoveComponent(LevelComponentsLookup.AnyTeamGameRankingAddedListener);
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
public sealed partial class LevelMatcher
{
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherAnyTeamGameRankingAddedListener;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> AnyTeamGameRankingAddedListener
	{
		get
		{
			if (_matcherAnyTeamGameRankingAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.AnyTeamGameRankingAddedListener);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherAnyTeamGameRankingAddedListener = matcher;
			}

			return _matcherAnyTeamGameRankingAddedListener;
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
public partial class LevelEntity
{
	public void AddAnyTeamGameRankingAddedListener(IAnyTeamGameRankingAddedListener value)
	{
		var listeners = HasAnyTeamGameRankingAddedListener
			? AnyTeamGameRankingAddedListener.value
			: new System.Collections.Generic.List<IAnyTeamGameRankingAddedListener>();
		listeners.Add(value);
		ReplaceAnyTeamGameRankingAddedListener(listeners);
	}

	public void RemoveAnyTeamGameRankingAddedListener(IAnyTeamGameRankingAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = AnyTeamGameRankingAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveAnyTeamGameRankingAddedListener();
		}
		else
		{
			ReplaceAnyTeamGameRankingAddedListener(listeners);
		}
	}
}