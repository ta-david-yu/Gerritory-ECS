//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerStateEntity
{
	public StateTimerComponent StateTimer { get { return (StateTimerComponent)GetComponent(PlayerStateComponentsLookup.StateTimer); } }
	public bool HasStateTimer { get { return HasComponent(PlayerStateComponentsLookup.StateTimer); } }

	public void AddStateTimer(float newValue)
	{
		var index = PlayerStateComponentsLookup.StateTimer;
		var component = (StateTimerComponent)CreateComponent(index, typeof(StateTimerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceStateTimer(float newValue)
	{
		var index = PlayerStateComponentsLookup.StateTimer;
		var component = (StateTimerComponent)CreateComponent(index, typeof(StateTimerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyStateTimerTo(StateTimerComponent copyComponent)
	{
		var index = PlayerStateComponentsLookup.StateTimer;
		var component = (StateTimerComponent)CreateComponent(index, typeof(StateTimerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = copyComponent.Value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveStateTimer()
	{
		RemoveComponent(PlayerStateComponentsLookup.StateTimer);
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
public sealed partial class PlayerStateMatcher
{
	static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> _matcherStateTimer;

	public static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> StateTimer
	{
		get
		{
			if (_matcherStateTimer == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<PlayerStateEntity>)JCMG.EntitasRedux.Matcher<PlayerStateEntity>.AllOf(PlayerStateComponentsLookup.StateTimer);
				matcher.ComponentNames = PlayerStateComponentsLookup.ComponentNames;
				_matcherStateTimer = matcher;
			}

			return _matcherStateTimer;
		}
	}
}