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
	public StateComponent State { get { return (StateComponent)GetComponent(PlayerStateComponentsLookup.State); } }
	public bool HasState { get { return HasComponent(PlayerStateComponentsLookup.State); } }

	public void AddState(int newHolderId)
	{
		var index = PlayerStateComponentsLookup.State;
		var component = (StateComponent)CreateComponent(index, typeof(StateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.HolderId = newHolderId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceState(int newHolderId)
	{
		var index = PlayerStateComponentsLookup.State;
		var component = (StateComponent)CreateComponent(index, typeof(StateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.HolderId = newHolderId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyStateTo(StateComponent copyComponent)
	{
		var index = PlayerStateComponentsLookup.State;
		var component = (StateComponent)CreateComponent(index, typeof(StateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.HolderId = copyComponent.HolderId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveState()
	{
		RemoveComponent(PlayerStateComponentsLookup.State);
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
	static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> _matcherState;

	public static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> State
	{
		get
		{
			if (_matcherState == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<PlayerStateEntity>)JCMG.EntitasRedux.Matcher<PlayerStateEntity>.AllOf(PlayerStateComponentsLookup.State);
				matcher.ComponentNames = PlayerStateComponentsLookup.ComponentNames;
				_matcherState = matcher;
			}

			return _matcherState;
		}
	}
}
