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
	public SpeedChangeStateComponent SpeedChangeState { get { return (SpeedChangeStateComponent)GetComponent(PlayerStateComponentsLookup.SpeedChangeState); } }
	public bool HasSpeedChangeState { get { return HasComponent(PlayerStateComponentsLookup.SpeedChangeState); } }

	public void AddSpeedChangeState(float newSpeedMultiplier)
	{
		var index = PlayerStateComponentsLookup.SpeedChangeState;
		var component = (SpeedChangeStateComponent)CreateComponent(index, typeof(SpeedChangeStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpeedMultiplier = newSpeedMultiplier;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSpeedChangeState(float newSpeedMultiplier)
	{
		var index = PlayerStateComponentsLookup.SpeedChangeState;
		var component = (SpeedChangeStateComponent)CreateComponent(index, typeof(SpeedChangeStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpeedMultiplier = newSpeedMultiplier;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySpeedChangeStateTo(SpeedChangeStateComponent copyComponent)
	{
		var index = PlayerStateComponentsLookup.SpeedChangeState;
		var component = (SpeedChangeStateComponent)CreateComponent(index, typeof(SpeedChangeStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpeedMultiplier = copyComponent.SpeedMultiplier;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSpeedChangeState()
	{
		RemoveComponent(PlayerStateComponentsLookup.SpeedChangeState);
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
	static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> _matcherSpeedChangeState;

	public static JCMG.EntitasRedux.IMatcher<PlayerStateEntity> SpeedChangeState
	{
		get
		{
			if (_matcherSpeedChangeState == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<PlayerStateEntity>)JCMG.EntitasRedux.Matcher<PlayerStateEntity>.AllOf(PlayerStateComponentsLookup.SpeedChangeState);
				matcher.ComponentNames = PlayerStateComponentsLookup.ComponentNames;
				_matcherSpeedChangeState = matcher;
			}

			return _matcherSpeedChangeState;
		}
	}
}
