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
	public WaitingForRespawnStateComponent WaitingForRespawnState { get { return (WaitingForRespawnStateComponent)GetComponent(PlayerStateComponentsLookup.WaitingForRespawnState); } }
	public bool HasWaitingForRespawnState { get { return HasComponent(PlayerStateComponentsLookup.WaitingForRespawnState); } }

	public void AddWaitingForRespawnState(int newRespawnAreaId)
	{
		var index = PlayerStateComponentsLookup.WaitingForRespawnState;
		var component = (WaitingForRespawnStateComponent)CreateComponent(index, typeof(WaitingForRespawnStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.RespawnAreaId = newRespawnAreaId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceWaitingForRespawnState(int newRespawnAreaId)
	{
		var index = PlayerStateComponentsLookup.WaitingForRespawnState;
		var component = (WaitingForRespawnStateComponent)CreateComponent(index, typeof(WaitingForRespawnStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.RespawnAreaId = newRespawnAreaId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyWaitingForRespawnStateTo(WaitingForRespawnStateComponent copyComponent)
	{
		var index = PlayerStateComponentsLookup.WaitingForRespawnState;
		var component = (WaitingForRespawnStateComponent)CreateComponent(index, typeof(WaitingForRespawnStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.RespawnAreaId = copyComponent.RespawnAreaId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveWaitingForRespawnState()
	{
		RemoveComponent(PlayerStateComponentsLookup.WaitingForRespawnState);
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
	static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> _matcherWaitingForRespawnState;

	public static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> WaitingForRespawnState
	{
		get
		{
			if (_matcherWaitingForRespawnState == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<PlayerStateEntity>)JCMG.EntitasRedux.Matcher<PlayerStateEntity>.AllOf(PlayerStateComponentsLookup.WaitingForRespawnState);
				matcher.ComponentNames = PlayerStateComponentsLookup.ComponentNames;
				_matcherWaitingForRespawnState = matcher;
			}

			return _matcherWaitingForRespawnState;
		}
	}
}
